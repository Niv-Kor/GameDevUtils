using System;
using UnityEngine;

namespace GameDevUtils.ObjectManagement
{
    [Serializable]
    public struct WeightedElement<TElement>
    {
        [Tooltip("The weighted element.")]
        [SerializeField] public TElement Element;

        [Tooltip("The chance of the object to be selected from a list.")]
        [SerializeField] [Range(0f, 1f)] public float Weight;
    }
}