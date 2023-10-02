using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevUtils.ObjectManagement
{
    public abstract class Pool<TObject> : WeightedList<TObject> where TObject : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("The object that contains all created items (if empty - use the current object).")]
        [SerializeField] protected Transform parent;

        [Tooltip("The maximum allowed amount of items (active or non-active) at all times.")]
        [SerializeField] protected uint maxTotalAmount = 100;

        [Tooltip("An initial amount of objects to insert to the pool on awake.")]
        [SerializeField] protected int initialAmount;
        #endregion

        #region Class Members
        protected Queue<TObject> pool;
        protected int totalItems;
        #endregion

        #region Properties
        public Transform Parent => (parent != null) ? parent : transform;
        public bool MaxTotalExceeded => totalItems >= maxTotalAmount;
        public int PoolSize => pool?.Count ?? 0;
        #endregion

        protected virtual void Awake() {
            this.pool = new Queue<TObject>();
            this.totalItems = 0;
        }

        protected virtual void Start() {
            Insert(initialAmount);
        }

        /// <summary>
        /// Manually insert a fixed amount of items into the pool.
        /// </summary>
        /// <param name="amount">The amount of items to insert</param>
        /// <returns>The amount of successfully inserted items.</returns>
        protected virtual int Insert(int amount) {
            int inserted = 0;

            for (int i = 0; i < amount; i++) {
                if (!Make(out TObject item)) break;

                item.gameObject.SetActive(false);
                pool.Enqueue(item);
                inserted++;
            }

            return inserted;
        }

        /// <summary>
        /// Generate a randome element from the list.
        /// </summary>
        /// <param name="instance">The newly created item (or null if no more items can be created due to the limit being exceeded)</param>
        /// <param name="seed">A pseudo-random seed to use while selecting a prefab</param>
        /// <returns>True if an instance has been successfully made.</returns>
        protected virtual bool Make(out TObject instance, int? seed = null) {
            if (MaxTotalExceeded) {
                instance = null;
                return false;
            }

            TObject selected = Select(seed);

            //instantiate
            if (selected != null) {
                instance = Instantiate(selected, Parent);
                instance.name = instance.name.Replace("(Clone)", $"({instance.gameObject.GetHashCode()})");
                totalItems++;

                return true;
            }

            instance = null;
            return false;
        }
        
        /// <summary>
        /// Clear the pool.
        /// </summary>
        public virtual void Clear() {
            try {
                if (pool != null) {
                    while (pool.Count > 0) {
                        GameObject obj = pool.Dequeue()?.gameObject;
                        if (obj) DestroyImmediate(obj);
                    }

                    pool.Clear();
                }

                totalItems = 0;
            }
            catch (Exception) {}
        }

        /// <summary>
        /// Return an item to the pool.
        /// </summary>
        /// <param name="item">The item to return</param>
        public virtual void Return(TObject item) {
            if (item == null) return;

            if (!pool.Contains(item)) pool.Enqueue(item);
            item.gameObject.SetActive(false);
        }

        /// <summary>
        /// Take an item from the pool,
        /// or generate one if the pool is empty.
        /// </summary>
        /// <param name="instance">The retrieved item (or null if no more items can be created due to the limit being exceeded)</param>
        /// <returns>True if an instance has been successfully retrieved.</returns>
        public virtual bool Take(out TObject instance) {
            if (PoolSize > 0) {
                instance = pool.Dequeue();
                instance.gameObject.SetActive(true);
                return true;
            }
            else return Make(out instance);
        }
    }
}