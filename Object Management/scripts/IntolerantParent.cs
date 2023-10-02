using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameDevUtils.ObjectManagement
{
    public abstract class IntolerantParent<T> : Singleton<T> where T : IntolerantParent<T>
    {
        #region Class Members
        private IDictionary<DisposableChild, Coroutine> autoDisposalCoroutines;
        #endregion

        private void Awake() {
            this.autoDisposalCoroutines = new Dictionary<DisposableChild, Coroutine>();
        }

        /// <summary>
        /// Dispose a child object after a fixed amount of time [s].
        /// </summary>
        /// <param name="child">The child to dispose</param>
        /// <param name="time">The time to wait before disposing the child [s]</param>
        private IEnumerator DisposeAfter(DisposableChild child, float time) {
            yield return new WaitForSeconds(time);
            child.Dispose();
        }

        /// <summary>
        /// Clear a child object's standby disposal coroutine.
        /// </summary>
        /// <param name="child">The child whose disposal coroutine to clear</param>
        private void ClearChildAutoDisposal(DisposableChild child) {
            bool coroutineExists = autoDisposalCoroutines.TryGetValue(child, out Coroutine coroutine);

            if (coroutineExists && coroutine != null) {
                StopCoroutine(coroutine);
                autoDisposalCoroutines.Remove(child);
            }
        }

        /// <summary>
        /// Set this object as a parent.
        /// </summary>
        /// <param name="child">The object to set as a child</param>
        /// <param name="originParent">The original parent of the object</param>
        /// <param name="disposeAction">The action to execute when the child is disposed</param>
        /// <param name="disposalCallback">A callback to activate when the child is disposed</param>
        public void Adopt(
            DisposableChild child,
            Transform originParent = null,
            ChildDisposeAction disposeAction = ChildDisposeAction.None,
            UnityAction<DisposableChild> disposalCallback = null
        ) {
            child.transform.SetParent(transform);
            if (originParent != null) child.OriginParent = originParent;

            child.DisposalEvent += delegate {
                ClearChildAutoDisposal(child);

                switch (disposeAction) {
                    case ChildDisposeAction.Kill:
                        Destroy(child.gameObject);
                        break;

                    case ChildDisposeAction.Return:
                        Return(child);
                        break;
                }

                disposalCallback?.Invoke(child);
            };

            //auto dispose
            if (child.DisposeAfterSeconds >= 0) {
                ClearChildAutoDisposal(child);
                Coroutine coroutine = StartCoroutine(DisposeAfter(child, child.DisposeAfterSeconds));
                autoDisposalCoroutines[child] = coroutine;
            }
        }

        /// <summary>
        /// Return a child to its original parent.
        /// </summary>
        /// <param name="child">The child to return</param>
        public void Return(DisposableChild child) {
            if (child.transform.parent != transform || child.OriginParent == null) return;
            child.transform.SetParent(child.OriginParent);
        }
    }
}