using UnityEngine;

namespace GameDevUtils.UI
{
    public class DeltaSizeUIItemScaler : UIItemScaler
    {
        private enum DeltaFactor
        {
            WidthAndHeight,
            Width,
            Height
        }

        #region Exposed Editor Parameters
        [Header("Factors")]
        [Tooltip("The parameter to scale.")]
        [SerializeField] private DeltaFactor deltaFactor;
        #endregion

        /// <inheritdoc/>
        protected override Vector2 CalcScale(Vector2 originScale, float percent) {
            bool isBoth = deltaFactor == DeltaFactor.WidthAndHeight;
            Vector2 delta = originScale;

            if (deltaFactor == DeltaFactor.Width || isBoth) delta.x *= percent;
            if (deltaFactor == DeltaFactor.Width || isBoth) delta.y *= percent;

            return delta;
        }

        /// <inheritdoc/>
        protected override void Scale(Vector2 newScale) {
            rect.sizeDelta = newScale;
        }
    }
}