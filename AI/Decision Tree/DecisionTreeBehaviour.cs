using UnityEngine;

namespace GameDevUtils.AI.DecisionTree
{
    public abstract class DecisionTreeBehaviour : MonoBehaviour
    {
        #region Properties
        public abstract string XMLTreeName { get; }
        #endregion
    }
}