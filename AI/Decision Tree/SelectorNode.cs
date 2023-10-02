using GameDevUtils.Math;
using GameDevUtils.ObjectManagement;
using System.Collections.Generic;
using System.Linq;

namespace GameDevUtils.AI.DecisionTree
{
    public abstract class SelectorNode : DecisionTreeNode
    {
        #region Class Members
        protected DecisionTreeNode lastSelectedChild;
        #endregion

        /// <inheritdoc/>
        protected SelectorNode(string name, float weight = 1) : base(name, weight) {}

        /// <summary>
        /// Normalize the children's weights.
        /// </summary>
        /// <returns>A generated weighted list of this node's children.</returns>
        protected virtual List<WeightedElement<DecisionTreeNode>> FixWeights() {
            List<WeightedElement<DecisionTreeNode>> weightedList = (from child in Children
                                                                    select new WeightedElement<DecisionTreeNode> {
                                                                        Element = child,
                                                                        Weight = child.Weight
                                                                    }).ToList();

            NormalizeList(weightedList);
            foreach (WeightedElement<DecisionTreeNode> child in weightedList)
                child.Element.SetWeight(child.Weight);

            return weightedList;
        }

        /// <summary>
        /// Select a weighted child.
        /// </summary>
        /// <returns>The selected child.</returns>
        protected virtual DecisionTreeNode Select() {
            List<WeightedElement<DecisionTreeNode>> weightedList = FixWeights();
            return weightedList.Generate();
        }

        /// <inheritdoc/>
        protected override DecisionStatus ProcessNode() {
            lastSelectedChild ??= Select();

            DecisionStatus status = lastSelectedChild.Process();
            if (status == DecisionStatus.Success) lastSelectedChild = null;

            return status;
        }

        /// <summary>
        /// Normalize the weights of the list so they fit the selector's logic.
        /// </summary>
        /// <param name="list"></param>
        protected abstract void NormalizeList(List<WeightedElement<DecisionTreeNode>> list);
    }
}