using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameDevUtils.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIScreen<TScreenLayout> : MonoBehaviour where TScreenLayout : Enum
    {
        #region Exposed Editor Parameters
        [Header("Settings")]
        [Tooltip("The unique signature of the screen.")]
        [SerializeField] protected TScreenLayout screenLayout;

        [Header("Fade Delay")]
        [Tooltip("The time it takes the screen to start fading in.")]
        [SerializeField] protected float fadeInDelay = 0;

        [Tooltip("True to only wait once for the fade delay.")]
        [SerializeField] protected bool delayOnce;
        #endregion

        #region Class Members
        protected MultiscreenUI<TScreenLayout> multiscreenUI;
        protected CanvasGroup canvas;
        protected bool shouldDelayFadeIn;
        #endregion

        #region Properties
        public TScreenLayout Layout { get => screenLayout; }
        public bool IsPresent { get => canvas.alpha > 0 || canvas.blocksRaycasts; }
        public List<UIScreen<TScreenLayout>> ChildScreens { get; protected set; }
        public List<UIScreen<TScreenLayout>> ParentScreens { get; protected set; }
        public List<TScreenLayout> ChildScreensID { get; protected set; }
        public List<TScreenLayout> ParentScreensID { get; protected set; }
        protected MultiscreenUI<TScreenLayout> UI { get; private set; }
        #endregion

        protected virtual void Start() {
            this.UI = GetComponentInParent<MultiscreenUI<TScreenLayout>>();
            this.canvas = GetComponent<CanvasGroup>();
            this.ChildScreens = GetComponentsInChildren<UIScreen<TScreenLayout>>().ToList();
            this.ParentScreens = GetComponentsInParent<UIScreen<TScreenLayout>>().ToList();
            ChildScreens.Remove(this);
            ParentScreens.Remove(this);
            this.shouldDelayFadeIn = true;
            this.ChildScreensID = (from screen in ChildScreens
                                   select screen.Layout).ToList();

            this.ParentScreensID = (from screen in ParentScreens
                                    select screen.Layout).ToList();
        }

        /// <summary>
        /// Activate whenever this screen appears.
        /// </summary>
        /// <param name="prevScreen">The changed screen</param>
        protected abstract void OnScreenUp(UIScreen<TScreenLayout> prevScreen);

        /// <summary>
        /// Activate whenever this screen changes to another.
        /// </summary>
        /// <param name="nextScreen">The next upcoming screen</param>
        protected abstract void OnScreenOff(UIScreen<TScreenLayout> nextScreen);

        /// <summary>
        /// Gradually fade the screen in or out.
        /// </summary>
        /// <param name="fadeIn">True to fade the sceen in (make it appear)</param>
        /// <param name="time">The time it takes to fade the screen</param>
        /// <param name="contextScreen">The previous screen (if fading in) or next screen (if fading out)</param>
        protected virtual IEnumerator Fade(bool fadeIn, float time, UIScreen<TScreenLayout> contextScreen) {
            if (!fadeIn) canvas.blocksRaycasts = false;
            float timer;

            //wait
            if (fadeIn && shouldDelayFadeIn) {
                timer = fadeInDelay;

                while (timer > 0) {
                    timer -= Time.deltaTime;
                    yield return null;
                }

                if (delayOnce) shouldDelayFadeIn = false;
            }

            float from = canvas.alpha;
            float to = fadeIn ? 1 : 0;
            timer = 0;

            while (timer <= time) {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(from, to, timer / time);
                yield return null;
            }

            if (fadeIn) canvas.blocksRaycasts = true;

            //activate callbacks
            if (contextScreen != null) {
                if (fadeIn) OnScreenUp(contextScreen);
                else OnScreenOff(contextScreen);
            }
        }

        /// <summary>
        /// Gradually fade the screen in or out.
        /// </summary>
        /// <param name="fadeIn">True to fade the sceen in (make it appear)</param>
        /// <param name="time">The time it takes to fade the screen</param>
        public void ChangeScreen(bool fadeIn, float time, UIScreen<TScreenLayout> contextScreen) {
            //display all parent screens
            if (fadeIn) {
                foreach (UIScreen<TScreenLayout> screen in ParentScreens)
                    if (!screen.IsPresent) screen.ChangeScreen(true, time, contextScreen);
            }
            //dismiss all child screens
            else {
                foreach (UIScreen<TScreenLayout> screen in ChildScreens)
                    if (screen.IsPresent) screen.ChangeScreen(false, time, contextScreen);
            }

            StopAllCoroutines();
            StartCoroutine(Fade(fadeIn, time, contextScreen));
        }
    }
}