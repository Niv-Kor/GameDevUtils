using UnityEngine;
using UnityEngine.Events;

namespace GameDevUtils.ObjectManagement
{
    public abstract class DisposableChild : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("True to enable disposing this object after a fixed amount of time.")]
        [SerializeField] private bool timedDispose;

        [Tooltip("The amount of time to wait before automatically disposing this object [s] (only relevant if 'timedDispose' is true).")]
        [SerializeField] private float disposeAfterSeconds;
        #endregion

        #region Properties
        public Transform OriginParent { get; set; }
        public float DisposeAfterSeconds => timedDispose ? disposeAfterSeconds : -1;
        #endregion

        #region Events
        public event UnityAction DisposalEvent;
        #endregion

        /// <summary>
        /// Activate this method before the Dispose event fires.
        /// </summary>
        protected virtual void BeforeDisposal() {}

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose() {
            BeforeDisposal();
            DisposalEvent?.Invoke();
        }
    }
}