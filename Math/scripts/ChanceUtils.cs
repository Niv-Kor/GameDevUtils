using GameDevUtils.ObjectManagement;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace GameDevUtils.Math
{
    public static class ChanceUtils
    {
        #region Class Members
        private static readonly Random defaultGenerator = new Random();
        private static readonly ConcurrentDictionary<int, Random> seededGenerators = new ConcurrentDictionary<int, Random>();
        #endregion

        /// <param name="seed">The generator's seed</param>
        /// <returns>The respective seeded generator, or the default one if no seed has been passed</returns>
        private static Random GetGenerator(int? seed = null) {
            if (!seed.HasValue) return defaultGenerator;
            else return seededGenerators.GetOrAdd(seed.Value, seed => new Random(seed));
        }

        /// <param name="seed">The seed of the generator to reset</param>
        public static void ResetSeededGenerator(int seed) {
            seededGenerators.TryRemove(seed, out Random _);
        }

        /// <summary>
        /// Generate a random boolean output.
        /// </summary>
        /// <param name="chance">The chance of the answer being True [0:1]</param>
        /// <param name="seed">A pseudo-random seed to use</param>
        /// <returns>A random boolean output, based on the specified chance.</returns>
        public static bool UnstableCondition(float chance, int? seed = null) {
            if (chance > 1) chance /= 100f;
            chance = Mathf.Clamp01(chance);

            switch (chance) {
                case 0: return false;
                case 1: return true;
                default: return GetGenerator(seed).Value() <= chance;
            }
        }

        /// <summary>
        /// Generate a random boolean answer.
        /// </summary>
        /// <param name="seed">A pseudo-random seed to use</param>
        /// <returns>A completely random boolean output.</returns>
        public static bool UnstableCondition(int? seed = null) => UnstableCondition(GetGenerator(seed).Value());

        /// <summary>
        /// Sum all weights of a weighted list.
        /// </summary>
        /// <typeparam name="TElement">The list's element type</typeparam>
        /// <returns>The sum of all weights.</returns>
        public static float SumWeights<TElement>(this List<WeightedElement<TElement>> list) =>
            (from obj in list select obj.Weight).Sum();

        /// <summary>
        /// Selected a random element from a weighted list.
        /// </summary>
        /// <typeparam name="TElement">The list's element type</typeparam>
        /// <param name="seed">A pseudo-random seed</param>
        /// <returns>The selected element.</returns>
        public static TElement Generate<TElement>(this List<WeightedElement<TElement>> list, int? seed = null) {
            //sort a list from the heaviest weight to the lightest
            var sortedList = new List<WeightedElement<TElement>>(list);
            sortedList.Sort((a, b) => {
                bool larger = a.Weight > b.Weight;

                if (a.Weight > b.Weight) return -1;
                else if (a.Weight < b.Weight) return 1;
                else return 0;
            });

            float sum = sortedList.SumWeights();
            TElement selected = default;
            Random generator = GetGenerator(seed);

            foreach (WeightedElement<TElement> weightedObj in sortedList) {
                float rnd = generator.Range(0, sum);
                float weight = weightedObj.Weight;

                if (rnd < weight) {
                    selected = weightedObj.Element;
                    break;
                }

                sum -= weight;
            }

            return selected ?? sortedList[sortedList.Count - 1].Element;
        }

        /// <summary>
        /// Selected a random element from a list.
        /// </summary>
        /// <typeparam name="TElement">The list's element type</typeparam>
        /// <param name="seed">A pseudo-random seed</param>
        /// <returns>The selected element.</returns>
        public static TElement Generate<TElement>(this List<TElement> list, int? seed = null) {
            if (list.Count == 0) return default;

            int rndIndex = GetGenerator(seed).Range(0, list.Count);
            return list[rndIndex];
        }

        /// <summary>
        /// Squeeze or expand a weighted list's weights so they fill exactly 100% [1].
        /// </summary>
        /// <typeparam name="TElement">The list's element type</typeparam>
        public static void SqueezeWeights<TElement>(this List<WeightedElement<TElement>> list) {
            float sum = list.SumWeights();
            int multiplier = (sum < 1) ? 1 : -1;
            float excess = (1 - sum) * multiplier;
            float extraWeightPerElement = excess / list.Count * multiplier;

            for (int i = 0; i < list.Count; i++) {
                WeightedElement<TElement> currentElement = list[i];
                list[i] = new WeightedElement<TElement> {
                    Element = currentElement.Element,
                    Weight = Mathf.Clamp(currentElement.Weight + extraWeightPerElement, 0, 1)
                };
            }
        }

        /// <summary>
        /// Make all elements of a weighted list weight the same.
        /// </summary>
        /// <typeparam name="TElement">The list's element type</typeparam>
        public static void EvenOut<TElement>(this List<WeightedElement<TElement>> list) {
            float evenOdd = 1f / list.Count;

            for (int i = 0; i < list.Count; i++) {
                WeightedElement<TElement> currentElement = list[i];
                list[i] = new WeightedElement<TElement> {
                    Element = currentElement.Element,
                    Weight = Mathf.Clamp(evenOdd, 0, 1)
                };
            }
        }

        /// <summary>
        /// Generate a random value from a vector2 range.
        /// </summary>
        /// <param name="vector">
        /// The vector from which to generate a value,
        /// where X is the minimum value and Y is the maximum value.
        /// </param>
        /// <returns>A random value within the vector's range.</returns>
        public static float GenerateValue(this Vector2 vector, int? seed = null) => Range(vector.x, vector.y, seed);

        /// <returns>A random float value [0:1].</returns>
        public static float Value(this Random rng) => (float)rng.NextDouble();

        /// <param name="min">Minimum value (inclusive)</param>
        /// <param name="max">Maximum value (exclusive)</param>
        /// <returns>A value within the range of the two given values [min:max)</returns>
        public static float Range(this Random rng, float min, float max) =>
            rng.Value() * (max - min) + min;

        /// <see cref="Range(Random, float, float)" />
        public static int Range(this Random rng, int min, int max) => rng.Next(min, max);

        /// <param name="seed">The pseudo-random seed to use</param>
        /// <see cref="Range(Random, float, float)" />
        public static float Range(float min, float max, int? seed = null) => GetGenerator(seed).Range(min, max);

        /// <param name="seed">The pseudo-random seed to use</param>
        /// <see cref="Range(Random, int, int)" />
        public static int Range(int min, int max, int? seed = null) => GetGenerator(seed).Range(min, max);
    }
}