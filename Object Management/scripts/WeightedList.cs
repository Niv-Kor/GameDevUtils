using GameDevUtils.Math;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameDevUtils.ObjectManagement
{
    public abstract class WeightedList<TObject> : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("Use this property to drag a batch of objects and automatically populate the weighted list.")]
        [SerializeField] private List<TObject> listPopulator;

        [Tooltip("A list of weighted elements.")]
        [SerializeField] protected List<WeightedElement<TObject>> elements;

        [Tooltip("True to make the odds of all objects in the list even.")]
        [SerializeField] protected bool useEvenOdds;
        #endregion

        private void OnValidate() {
            //populate the list
            if (listPopulator.Count > 0) {
                foreach (TObject obj in listPopulator)
                    elements.Add(new WeightedElement<TObject> { Element = obj });

                listPopulator.Clear();
            }

            //fix weights
            if (useEvenOdds) ChanceUtils.EvenOut(elements);
            else ChanceUtils.SqueezeWeights(elements);
        }

        /// <param name="seed">The pseudo-random seed by which to select an element</param>
        /// <returns>A random element from the list.</returns>
        public TObject Select(int? seed = null) {
            if (elements.Count > 0) return elements.Generate(seed);
            else throw new UnassignedReferenceException($"List is empty (GameObject name: {gameObject.name}).");
        }

        /// <param name="match">A function that matches the desired element</param>
        /// <returns>The desired element from the list, regardless of its weight.</returns>
        public TObject Find(System.Predicate<TObject> match) {
            List<TObject> rawElements = (from WeightedElement<TObject> weightedEl in elements select weightedEl.Element).ToList();
            return rawElements.Find(match);
        }
    }
}
