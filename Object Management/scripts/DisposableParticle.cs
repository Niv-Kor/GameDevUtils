using UnityEngine;
using UnityEngine.Events;

namespace GameDevUtils.ObjectManagement
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DisposableParticle : DisposableChild
    {
        #region Class Members
        protected ParticleSystem partSys;
        protected UnityAction disposalCallback;
        #endregion

        protected virtual void Awake() {
            this.partSys = GetComponent<ParticleSystem>();
        }

        protected virtual void Start() {
            ParticleSystem.MainModule mainModule = partSys.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        protected virtual void OnParticleSystemStopped() {
            disposalCallback?.Invoke();
            Dispose();
        }

        /// <summary>
        /// Play the particle system's effect.
        /// </summary>
        /// <param name="callback">A callback function to activate when the particles are disposed</param>
        public virtual void PlayEffect(UnityAction callback = null) {
            disposalCallback = callback;
            partSys.Play();
        }
    }
}