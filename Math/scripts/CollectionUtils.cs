using System.Collections.Generic;

namespace GameDevUtils.Math
{
    public static class CollectionUtils
    {
        /// <summary>
        /// Generate a a random element from a list.
        /// </summary>
        /// <typeparam name="TElement">The element's type</typeparam>
        /// <param name="list">The list from which to generate an element</param>
        /// <returns>A random element from the given list, or a default value if the list is empty.</returns>
        public static TElement GenerateElement<TElement>(this List<TElement> list) {
            if (list == null || list.Count == 0) return default;
            else return list[ChanceUtils.Range(0, list.Count)];
        }

        /// <summary>
        /// Permanently pull a random element from a list without returning it.
        /// </summary>
        /// <typeparam name="TElement">The element's type</typeparam>
        /// <param name="list">The list from which to pull the element</param>
        /// <returns>A random element from the given list, or a default value if the list is empty.</returns>
        public static TElement PullRandom<TElement>(this List<TElement> list) {
            if (list == null || list.Count == 0) return default;

            int index = ChanceUtils.Range(0, list.Count);
            TElement element = list[index];
            list.RemoveAt(index);
            return element;
        }

        /// <summary>
        /// Check if a list's sequence contains another list's entire content.
        /// </summary>
        /// <typeparam name="TElement">The element's type</typeparam>
        /// <param name="listA">The list that should contain list B</param>
        /// <param name="listB">The list that should be contained inside list A</param>
        /// <returns>True if list A contains a sequence that equals list B's entire content.</returns>
        public static bool ContainsList<TElement>(this List<TElement> listA, List<TElement> listB) {
            if (listA == null || listB == null || listB.Count == 0) return false;

            int indexA = 0;
            int indexB = 0;

            while (indexA < listA.Count && indexB < listB.Count) {
                TElement a = listA[indexA];
                TElement b = listB[indexB];
                bool equal = EqualityComparer<TElement>.Default.Equals(a, b);

                if (equal) indexB++;
                else indexB = 0;

                indexA++;
            }

            return indexB == listB.Count;
        }
    }
}