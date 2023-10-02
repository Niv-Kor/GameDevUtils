using GameDevUtils.ObjectManagement;
using GameDevUtils.Math;
using System.Collections.Generic;

namespace GameDevUtils.AI.DecisionTree
{
    public class WeightedSelectorNode : SelectorNode
    {
        /// <inheritdoc/>
        public WeightedSelectorNode(string name, float weight = 1) : base(name, weight) {}

        /// <inheritdoc/>
        protected override void NormalizeList(List<WeightedElement<DecisionTreeNode>> list) => list.SqueezeWeights();
    }
}