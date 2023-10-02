using UnityEngine;

namespace GameDevUtils.Math
{
    public static class ColorUtils
    {
        /// <param name="amount">The amount by which to brighten the color [0:1]</param>
        /// <returns>A brightened color.</returns>
        public static Color Brighten(this Color color, float amount) {
            color.r = Mathf.Clamp01(color.r + amount);
            color.g = Mathf.Clamp01(color.g + amount);
            color.b = Mathf.Clamp01(color.b + amount);
            return color;
        }

        /// <param name="amount">The amount by which to darken the color [0:1]</param>
        /// <returns>A darkened color.</returns>
        public static Color Darken(this Color color, float amount) => color.Brighten(-amount);
    }
}
