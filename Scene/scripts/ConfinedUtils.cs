using GameDevUtils.Math;
using UnityEngine;

namespace GameDevUtils.Scene
{
    public static class ConfineUtils
    {
        #region Constants
        private static readonly Color DEFAULT_COLOR = new Color(0xff, 0x0, 0xe8);
        #endregion

        /// <returns>The center world point of the confine.</returns>
        public static Vector3 GetWorldPointCenter(this Confine confine) => confine.Offset + confine.Size / 2;

        /// <returns>The maximal perimeter of the confine.</returns>
        public static float GetMaxPerimeter(this Confine confine) {
            float x = confine.Size.x;
            float y = confine.Size.y;
            float z = confine.Size.z;
            return Mathf.Max(x, y, z);
        }

        /// <summary>
        /// Draw the confine's borders.
        /// This function can only be used inside <see cref="MonoBehaviour.OnDrawGizmos()" />.
        /// </summary>
        /// <param name="confine"></param>
        /// <param name="color"></param>
        public static void Draw(this Confine confine, Color? color = null) {
            Gizmos.color = color ?? DEFAULT_COLOR;
            Gizmos.DrawWireCube(confine.GetWorldPointCenter(), confine.Size);
        }

        /// <summary>
        /// Check if a confine intersects another confine at any point.
        /// </summary>
        /// <param name="point">The other confine to check</param>
        /// <returns>True if the given confines intersect.</returns>
        public static bool Intersects(this Confine a, Confine b) {
            Vector3 aMin = a.Offset;
            Vector3 aMax = a.Offset + a.Size;
            Vector3 bMin = b.Offset;
            Vector3 bMax = b.Offset + b.Size;

            return aMin.x <= bMax.x &&
                   aMax.x >= bMin.x &&
                   aMin.y <= bMax.y &&
                   aMax.y >= bMin.y &&
                   aMin.z <= bMax.z &&
                   aMax.z >= bMin.z;
        }

        /// <summary>
        /// Check if a confine contains a world position.
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>True if the point is located within the confine's borders.</returns>
        public static bool Contains(this Confine confine, Vector3 point) {
            float xLen = confine.Offset.x + confine.Size.x;
            float yLen = confine.Offset.y + confine.Size.y;
            float zLen = confine.Offset.z + confine.Size.z;
            bool x = point.x >= confine.Offset.x && point.x <= xLen;
            bool y = point.y >= confine.Offset.y && point.y <= yLen;
            bool z = point.z >= confine.Offset.z && point.z <= zLen;
            return x && y && z;
        }

        /// <returns>A random position within the confine's borders.</returns>
        public static Vector3 GeneratePosition(this Confine confine) {
            float xLen = confine.Offset.x + confine.Size.x;
            float yLen = confine.Offset.y + confine.Size.y;
            float zLen = confine.Offset.z + confine.Size.z;
            float x = ChanceUtils.Range(confine.Offset.x, xLen);
            float y = ChanceUtils.Range(confine.Offset.y, yLen);
            float z = ChanceUtils.Range(confine.Offset.z, zLen);
            return new Vector3(x, y, z);
        }
    }
}