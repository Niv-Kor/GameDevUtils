using UnityEngine;

namespace GameDevUtils.Math
{
    public static class VectorUtils
    {
        #region Constants
        private static readonly float DEFAULT_MIN_ROTATION = -360;
        private static readonly float DEFAULT_MAX_ROTATION = 360;
        private static readonly Vector2 DEFAULT_PITCH_RANGE = new Vector2(DEFAULT_MIN_ROTATION, DEFAULT_MAX_ROTATION);
        private static readonly Vector2 DEFAULT_YAW_RANGE = new Vector2(DEFAULT_MIN_ROTATION, DEFAULT_MAX_ROTATION);
        private static readonly Vector2 DEFAULT_ROLL_RANGE = new Vector2(DEFAULT_MIN_ROTATION, DEFAULT_MAX_ROTATION);
        public static readonly Vector3 X_MASK = Vector3.right;
        public static readonly Vector3 Y_MASK = Vector3.up;
        public static readonly Vector3 Z_MASK = Vector3.forward;
        public static readonly Vector3 XY_MASK = X_MASK + Y_MASK;
        public static readonly Vector3 XZ_MASK = X_MASK + Z_MASK;
        public static readonly Vector3 YZ_MASK = Y_MASK + Z_MASK;
        #endregion

        /// <summary>
        /// Check if a vector has already reached a certain percentage of the distance it should make.
        /// </summary>
        /// <param name="destPos">The position that the vector should finally reach</param>
        /// <param name="distance">The total distance that's needed to be done</param>
        /// <param name="tolerance">A percentage of the total distance [0:1]</param>
        /// <returns>True if the vector has already reached the specified percentage of the distance.</returns>
        public static bool EffectivelyReached(this Vector3 pos, Vector3 destPos, float distance, float tolerance) {
            return EffectivelyReached(pos, destPos, tolerance * distance);
        }

        /// <summary>
        /// Check if a vector has already reached a certain percentage of the distance it should make.
        /// </summary>
        /// <param name="destPos">The position that the vector should finally reach</param>
        /// <param name="toleranceUnits">A distance between the two vectors that can be ignored</param>
        /// <returns>True if the vector has already reached the specified percentage of the distance.</returns>
        public static bool EffectivelyReached(this Vector3 pos, Vector3 destPos, float toleranceUnits) {
            float dist = Vector3.Distance(pos, destPos);
            return dist <= toleranceUnits;
        }

        /// <summary>
        /// Check if a quaternion has already reached a certain percentage of the rotation it should make.
        /// </summary>
        /// <param name="destRot">The rotation that the quaternion should finally reach</param>
        /// <param name="toleranceAngle">An angle between the two quaternions that can be ignored</param>
        /// <returns>True if the quaternion has already reached the specified percentage of the rotation.</returns>
        public static bool EffectivelyReached(this Quaternion rot, Quaternion destRot, float toleranceAngle) {
            float angle = Quaternion.Angle(rot, destRot);
            return angle <= toleranceAngle;
        }

        /// <summary>
        /// Clamp a vector's coordinates.
        /// Each axis is only compared against itself in the min/max vectors.
        /// </summary>
        /// <param name="min">Minimum vactor values</param>
        /// <param name="max">Maximum vector values</param>
        /// <returns>The given vector with clamped values.</returns>
        public static Vector2 Clamp(this Vector2 vector, Vector2 min, Vector2 max) {
            float x = Mathf.Clamp(vector.x, min.x, max.x);
            float y = Mathf.Clamp(vector.y, min.y, max.y);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Clamp a vector's coordinates.
        /// Each axis is only compared against itself in the min/max vectors.
        /// </summary>
        /// <param name="min">Minimum vactor values</param>
        /// <param name="max">Maximum vector values</param>
        /// <returns>The given vector with clamped values.</returns>
        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max) {
            float x = Mathf.Clamp(vector.x, min.x, max.x);
            float y = Mathf.Clamp(vector.y, min.y, max.y);
            float z = Mathf.Clamp(vector.z, min.z, max.z);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Generate a random rotation.
        /// </summary>
        /// <param name="xLim">A pitch range to randomly select from</param>
        /// <param name="xLim">A yaw range to randomly select from</param>
        /// <param name="xLim">A roll range to randomly select from</param>
        /// <returns>A random rotation quaternion.</returns>
        public static Quaternion GenerateRotation(Vector2? xLim = null, Vector2? yLim = null, Vector2? zLim = null) {
            Vector2 xLimFinal = xLim ?? DEFAULT_PITCH_RANGE;
            Vector2 yLimFinal = yLim ?? DEFAULT_YAW_RANGE;
            Vector2 zLimFinal = zLim ?? DEFAULT_ROLL_RANGE;

            float pitch = ChanceUtils.Range(xLimFinal.x, xLimFinal.y);
            float yaw = ChanceUtils.Range(yLimFinal.x, yLimFinal.y);
            float roll = ChanceUtils.Range(zLimFinal.x, zLimFinal.y);
            Vector3 euler = new Vector3(pitch, yaw, roll);
            return Quaternion.Euler(euler);
        }

        /// <summary>
        /// Convert each of the vector's positive values to 1
        /// and negative values to -1.
        /// </summary>
        /// <returns>A 3D vector that consists of values from the set {0, 1, -1}.</returns>
        public static Vector3Int StrongNormalize(this Vector3 vector) {
            int x = (vector.x > 0) ? 1 : (vector.x < 0) ? -1 : 0;
            int y = (vector.y > 0) ? 1 : (vector.y < 0) ? -1 : 0;
            int z = (vector.z > 0) ? 1 : (vector.z < 0) ? -1 : 0;
            return new Vector3Int(x, y, z);
        }

        /// <see cref="StrongNormalize(Vector3)"/>
        /// <returns>A 2D vector that consists of values from the set {0, 1, -1}.</returns>
        public static Vector2Int StrongNormalize(this Vector2 vector) =>
            (Vector2Int)((Vector3)vector).StrongNormalize();

        /// <summary>
        /// Round down all values of a vector.
        /// </summary>
        /// <returns>A 3D vector with rounded down values.</returns>
        public static Vector3 Floor(this Vector3 vector) {
            float x = Mathf.Floor(vector.x);
            float y = Mathf.Floor(vector.y);
            float z = Mathf.Floor(vector.z);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Round down all values of a vector.
        /// </summary>
        /// <returns>A 2D vector with rounded down values.</returns>
        public static Vector2 Floor(this Vector2 vector) =>
            ((Vector3)vector).Floor();

        /// <summary>
        /// Round up all values of a vector.
        /// </summary>
        /// <returns>A 3D vector with rounded up values.</returns>
        public static Vector3 Ceil(this Vector3 vector) {
            float x = Mathf.Ceil(vector.x);
            float y = Mathf.Ceil(vector.y);
            float z = Mathf.Ceil(vector.z);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Round up all values of a vector.
        /// </summary>
        /// <returns>A 2D vector with rounded up values.</returns>
        public static Vector2 Ceil(this Vector2 vector) =>
            ((Vector3)vector).Ceil();

        /// <summary>
        /// Check if all of the vector's signs match the signs of another vector.
        /// Each axis is checked against its equivalent.
        /// </summary>
        /// <param name="v2">The vector against which to check</param>
        /// <returns>True if all signs match.</returns>
        public static bool SameSign(this Vector3 v1, Vector3 v2) {
            bool x = (v1.x == 0 && v2.x == 0) || (v1.x != 0) && Mathf.Sign(v1.x) == Mathf.Sign(v2.x);
            bool y = (v1.y == 0 && v2.y == 0) || (v1.y != 0) && Mathf.Sign(v1.y) == Mathf.Sign(v2.y);
            bool z = (v1.z == 0 && v2.z == 0) || (v1.z != 0) && Mathf.Sign(v1.z) == Mathf.Sign(v2.z);
            return x & y & z;
        }

        /// <see cref="SameSign(Vector3, Vector3)"/>
        public static bool SameSign(this Vector2 v1, Vector2 v2) {
            return ((Vector3)v1).SameSign(v2);
        }

        /// <summary>
        /// Check if all of the vector's values are greater than the other's,
        /// while 'greater' means they must be obtainable by duplicating
        /// the subject values by a positive number.
        /// </summary>
        /// <example>
        /// 9 is greater than 3;
        /// -9 is greater than -3;
        /// (8, -2, 0) is greater than (7.8, -1, 0);
        /// (23, 4, -24) is NOT greater than (8, -2, -40)
        /// </example>
        /// <param name="v2">The subject vector against which to check</param>
        /// <param name="detailedTest">
        /// An array of flags that indicate which axis is actually greater than the other,
        /// according to the definition of 'greater'.
        /// </param>
        /// <returns>True if the vector is greater than the subject vector.</returns>
        public static bool GreaterThan(this Vector3 v1, Vector3 v2, out bool[] detailedTest) {
            detailedTest = new bool[3] {
                v1.x / v2.x >= 1,
                v1.y / v2.y >= 1,
                v1.z / v2.z >= 1
            };

            bool x = v2.x == 0 || detailedTest[0];
            bool y = v2.y == 0 || detailedTest[1];
            bool z = v2.z == 0 || detailedTest[2];
            return x & y & z;
        }

        /// <see cref="GreaterThan(Vector3, Vector3, out bool[])"/>
        public static bool GreaterThan(this Vector3 v1, Vector3 v2) => v1.GreaterThan(v2, out bool[] _);

        /// <see cref="GreaterThan(Vector3, Vector3, out bool[])"/>
        public static bool GreaterThan(this Vector2 v1, Vector2 v2) => ((Vector3)v1).GreaterThan(v2);

        /// <see cref="GreaterThan(Vector3, Vector3, out bool[])"/>
        public static bool GreaterThan(this Vector2 v1, Vector2 v2, out bool[] detailedTest) =>
            ((Vector3)v1).GreaterThan(v2, out detailedTest);

        /// <summary>
        /// Convert the vector to an absolute version of itself,
        /// where every axis converts to a positive value,
        /// if it's not already positive or 0.
        /// </summary>
        /// <returns>A vector with all values as absolute values.</returns>
        public static Vector3 Abs(this Vector3 vector) {
            float x = Mathf.Abs(vector.x);
            float y = Mathf.Abs(vector.y);
            float z = Mathf.Abs(vector.z);
            return new Vector3(x, y, z);
        }

        /// <see cref="Abs(Vector3)"/>
        public static Vector2 Abs(this Vector2 vector) => ((Vector3)vector).Abs();

        /// <summary>
        /// Perform an inverse Scale on 2 vectors.
        /// </summary>
        /// <param name="b">The vector by which to divide</param>
        /// <returns>The devision result.</returns>
        public static Vector3 Divide(this Vector3 a, Vector3 b) =>
            new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);

        /// <see cref="Divide(Vector3, Vector3)"/>
        public static Vector2 Divide(this Vector2 a, Vector2 b) => Divide((Vector3)a, (Vector3)b);

        /// <summary>
        /// Convert a 2D XZ vector to a 3D world vector (XZ -> XYZ).
        /// </summary>
        /// <param name="vector">The XZ vector to convert</param>
        /// <returns>An XYZ vector with no height.</returns>
        public static Vector3 XZToXYZ(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);

        /// <summary>
        /// Convert a 3D XYZ world vector to a 2D vector (XYZ -> XZ).
        /// </summary>
        /// <param name="vector">The XYZ vector to convert</param>
        /// <returns>An XZ vector where the Z in the previous Y.</returns>
        public static Vector2 XYZToXZ(this Vector3 vector) => new Vector2(vector.x, vector.z);

        /// <summary>
        /// Convert a 3D XYZ world vector to a 2D vector (XYZ -> XZ).
        /// </summary>
        /// <param name="vector">The XYZ vector to convert</param>
        /// <returns>An XZ vector where the Z in the previous Y.</returns>
        public static Vector2 XYZToXZ(this Vector3Int vector) => new Vector2(vector.x, vector.z);

        /// <summary>
        /// Swap a 2D vector's coordinates (XY -> YX).
        /// </summary>
        /// <param name="vector">The XY vector to convert</param>
        /// <returns>A YX vector.</returns>
        public static Vector2 XYToYX(this Vector2 vector) => new Vector2(vector.y, vector.x);

        /// <param name="vector">The vector to mask</param>
        /// <returns>The given vector, with its Y and Z values nullified.</returns>
        public static Vector3 MaskX(this Vector3 vector) => Vector3.Scale(vector, X_MASK);

        /// <param name="vector">The vector to mask</param>
        /// <returns>The given vector, with its X and Z values nullified.</returns>
        public static Vector3 MaskY(this Vector3 vector) => Vector3.Scale(vector, Y_MASK);

        /// <param name="vector">The vector to mask</param>
        /// <returns>The given vector, with its X and Y values nullified.</returns>
        public static Vector3 MaskZ(this Vector3 vector) => Vector3.Scale(vector, Z_MASK);

        /// <param name="vector">The vector to mask</param>
        /// <returns>The given vector, with its Z value nullified.</returns>
        public static Vector3 MaskXY(this Vector3 vector) => Vector3.Scale(vector, XY_MASK);

        /// <param name="vector">The vector to mask</param>
        /// <returns>The given vector, with its Y value nullified.</returns>
        public static Vector3 MaskXZ(this Vector3 vector) => Vector3.Scale(vector, XZ_MASK);

        /// <param name="vector">The vector to mask</param>
        /// <returns>The given vector, with its X value nullified.</returns>
        public static Vector3 MaskYZ(this Vector3 vector) => Vector3.Scale(vector, YZ_MASK);
    }
}