using UnityEngine;

namespace GameDevUtils.UI
{
    public class LocalScaleUIItemScaler : UIItemScaler
    {
        /// <inheritdoc/>
        protected override Vector2 CalcScale(Vector2 originScale, float percent) {
            return originScale * percent;
        }

        /// <inheritdoc/>
        protected override void Scale(Vector2 newScale) {
            rect.localScale = newScale;
        }
    }
}