using System.Linq;
using UnityEngine;

namespace GameDevUtils.Math
{
    public static class ImageProcessingUtils
    {
        #region Constants
        private static readonly int MAX_PERLIN_SEED = 100_000;
        #endregion

        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        /// <param name="matrix">The matrix to multiply</param>
        /// <param name="scalar">The scalar by which to multiply the matrix</param>
        /// <returns>A multiplied matrix.</returns>
        public static float[,] Multiply(float[,] matrix, float scalar) {
            if (scalar != 1)
                for (int x = 0; x < matrix.GetLength(0); x++)
                    for (int y = 0; y < matrix.GetLength(1); y++)
                        matrix[x, y] *= scalar;

            return matrix;
        }

        /// <summary>
        /// Generate a perlin noise map, where each value is between 0 and 1.
        /// </summary>
        /// <param name="width">The map's width</param>
        /// <param name="height">The map's height</param>
        /// <param name="scale">
        /// The scale of the map.
        /// the smaller the scale, the the more gradual are the values.
        /// </param>
        /// <returns>A perlin noise map.</returns>
        public static float[,] GeneratePerlinNoiseMap(int width, int height, float scale, int? seed = null) {
            float[,] map = new float[width, height];
            int perlinSeed = (seed != null) ? (int)seed % MAX_PERLIN_SEED : ChanceUtils.Range(1, MAX_PERLIN_SEED);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    float sampleX = (x + perlinSeed) * scale;
                    float sampleY = (y + perlinSeed) * scale;
                    map[x, y] = Mathf.PerlinNoise(sampleX, sampleY);
                }
            }

            return map;
        }

        /// <summary>
        /// Blur a matrix to smoothen its edges.
        /// </summary>
        /// <param name="matrix">The matrix to blur</param>
        /// <param name="kernel">The kernel to apply (must be smaller than the matrix)</param>
        /// <returns>A blurred version of the given matrix.</returns>
        public static float[,] Blur(float[,] matrix, float[,] kernel) {
            if (matrix.GetLength(0) < kernel.GetLength(0) + 2 || matrix.GetLength(1) < kernel.GetLength(1) + 2)
                throw new System.ArgumentException("The given matrix is too small to apply Gaussian Blur.");

            int matWidth = matrix.GetLength(0);
            int matHeight = matrix.GetLength(1);
            float kernelSum = kernel.Cast<float>().Sum();

            for (int x = 1; x < matWidth - 1; x++) {
                for (int y = 1; y < matHeight - 1; y++) {
                    float[,] sample = {
                        { matrix[x - 1, y - 1], matrix[x, y - 1], matrix[x + 1, y - 1] },
                        { matrix[x - 1, y], matrix[x, y], matrix[x + 1, y] },
                        { matrix[x - 1, y + 1], matrix[x, y + 1], matrix[x + 1, y + 1] }
                    };

                    float mean = 0;
                    for (int n = 0; n < sample.GetLength(0); n++)
                        for (int k = 0; k < sample.GetLength(1); k++)
                            mean += sample[n, k] * kernel[n, k];

                    matrix[x, y] = mean / kernelSum;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Apply a Gaussian Blur effect on a matrix.
        /// </summary>
        /// <param name="matrix">The matrix to blur</param>
        /// <param name="intensity">The effect's intensity</param>
        /// <returns>A matrix blurred with a Gaussian Blur.</returns>
        public static float[,] GaussianBlur(float[,] matrix, float intensity = 1) {
            float[,] kernel = {
                {1, 2, 1},
                {2, 4, 2},
                {1, 2, 1}
            };

            return Blur(matrix, Multiply(kernel, intensity));
        }

        /// <summary>
        /// Apply a Mean Blur effect on a matrix.
        /// </summary>
        /// <param name="matrix">The matrix to blur</param>
        /// <param name="intensity">The effect's intensity</param>
        /// <returns>A matrix blurred with a Mean Blur.</returns>
        public static float[,] MeanBlur(float[,] matrix, float intensity = 1) {
            float[,] kernel = {
                {1, 1, 1},
                {1, 1, 1},
                {1, 1, 1}
            };

            return Blur(matrix, Multiply(kernel, intensity));
        }
    }
}
