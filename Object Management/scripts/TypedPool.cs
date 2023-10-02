using System;
using UnityEngine;

namespace GameDevUtils.ObjectManagement
{
    public class TypedPool<TElement, TEnum> : Pool<TElement> where TElement : MonoBehaviour where TEnum : Enum
    {
        #region Exposed Editor Parameters
        [Tooltip("The type ID of element in the pool.")]
        [SerializeField] protected TEnum type;
        #endregion

        #region Properties
        public TEnum Type => type;
        #endregion
    }
}