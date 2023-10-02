using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevUtils.ObjectManagement
{
    public class PoolsHierarchy<TPool, TElement, TEnum> : WeightedList<TPool>
        where TPool : TypedPool<TElement, TEnum>
        where TElement : MonoBehaviour
        where TEnum : Enum
    {
        #region Class Members
        protected List<TPool> pools;
        #endregion

        protected virtual void Awake() {
            this.pools = new List<TPool>(GetComponentsInChildren<TPool>());
        }

        /// <param name="type">The pool's type</param>
        /// <returns>The pool that contains the given type of elements.</returns>
        public TPool GetPool(TEnum type) {
            TPool pool = Find(x => x.Type.ToString() == type.ToString());
            if (pool == null) throw new NullReferenceException($"Missing a typed pool for type \"{type}\".");

            return pool;
        }

        /// <param name="type">The type of element to pull</param>
        /// <param name="pool">The relevant typed pool from which the element was pulled</param>
        /// <param name="element">An element of the specified type from the relevant pool</param>
        /// <returns>True if an element has been successfully retrieved.</returns>
        public bool TryTakeElement(TEnum type, out TPool pool, out TElement element) {
            pool = GetPool(type);

            if (pool != null) return pool.Take(out element);
            else throw new NullReferenceException($"Missing a typed pool for type \"{type}\".");
        }

        /// <see cref="TryTakeElement(TEnum, out TPool, out TElement)"/>
        public bool TryTakeElement(TEnum type, out TElement element) => TryTakeElement(type, out TPool _, out element);

        /// <see cref="TryTakeElement(TEnum, out TPool, out TElement)"/>
        public bool TryTakeElement(out TPool pool, out TElement element) {
            pool = Select();

            if (pool != null) return pool.Take(out element);
            else throw new NullReferenceException("The hierarchy contains no pools.");
        }

        /// <see cref="TryTakeElement(TEnum, out TPool, out TElement)"/>
        public bool TryTakeElement(out TElement element) => TryTakeElement(out TPool _, out element);
    }
}