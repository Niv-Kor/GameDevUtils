namespace GameDevUtils.AI.DecisionTree
{
    public class DecisionTreeRoot : DecisionTreeNode
    {
        /// <inheritdoc/>
        public DecisionTreeRoot(string name, float weight = 1) : base(name, weight) {}

        /// <inheritdoc/>
        protected override DecisionStatus ProcessNode() => Children[childIndex].Process();
    }
}