namespace GameDevUtils.AI.DecisionTree
{
    public class SequenceNode : DecisionTreeNode
    {
        /// <inheritdoc/>
        public SequenceNode(string name, float weight = 1) : base(name, weight) {}

        /// <inheritdoc/>
        protected override DecisionStatus ProcessNode() {
            while (childIndex < Children.Count) {
                DecisionTreeNode child = Children[childIndex];
                DecisionStatus childStatus = child.Process();

                if (childStatus != DecisionStatus.Success) return childStatus;
                else childIndex++;
            }

            return DecisionStatus.Success;
        }
    }
}