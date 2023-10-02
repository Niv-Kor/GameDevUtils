using GameDevUtils.Math;
using GameDevUtils.ObjectManagement;
using System.Collections.Generic;

namespace GameDevUtils.AI.DecisionTree
{
    public class RandomSelectorNode : SelectorNode
    {
        /// <inheritdoc>
        public RandomSelectorNode(string name, float weight = 1) : base(name, weight) {}

        /// <inheritdoc>
        protected override void NormalizeList(List<WeightedElement<DecisionTreeNode>> list) => list.EvenOut();
    }
}