using UnityEngine;

namespace GameDevUtils.UI
{
    public abstract class UIItemScaler : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Header("Settings")]
        [Tooltip("An origin scale value that corresponds with the origin resolution.")]
        [SerializeField] protected Vector2 originScale;

        [Tooltip("The screen resolution for which the item's scale is the origin scale.")]
        [SerializeField] protected Vector2 originResolution;

        [Tooltip("True to immediately scale the component as the game starts.")]
        [SerializeField] protected bool scaleOnAwake = true;
        #endregion

        #region Class Members
        protected RectTransform rect;
        #endregion

        #region Properties
        public float ScaledPercent { get; private set; }
        public Vector2 AppropriateScale => CalcScale(originScale, ScaledPercent);
        #endregion

        protected virtual void Awake() {
            float originMagnitude = originResolution.magnitude;
            Vector2 resVec = new Vector2(Screen.width, Screen.height);
            float resMagnitude = resVec.magnitude;
            this.ScaledPercent = resMagnitude / originMagnitude;
            this.rect = GetComponent<RectTransform>();

            if (scaleOnAwake) Scale();
        }

        /// <summary>
        /// Scale the item.
        /// </summary>
        /// <param name="newScale">The new scale of the UI item</param>
        protected abstract void Scale(Vector2 newScale);

        /// <summary>
        /// Calculate the new scale of the item based on screen size.
        /// </summary>
        /// <param name="originScale">The original defined scale</param>
        /// <param name="percent">Percentage of the original defined scale</param>
        /// <returns>The new scale of the UI item.</returns>
        protected abstract Vector2 CalcScale(Vector2 originScale, float percent);

        /// <see cref="Scale(Vector2, float)"/>
        public void Scale() { Scale(AppropriateScale); }
    }
}