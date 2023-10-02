namespace GameDevUtils.AI.DecisionTree
{
    public class OrNode : DecisionTreeNode
    {
        /// <inheritdoc/>
        public OrNode(string name, float weight = 1) : base(name, weight) {}

        /// <inheritdoc/>
        protected override DecisionStatus ProcessNode() {
            int childrenFailed = 0;

            foreach (DecisionTreeNode child in Children) {
                DecisionStatus status = child.Process();

                if (status == DecisionStatus.Success) return DecisionStatus.Success;
                else if (status == DecisionStatus.Failure) childrenFailed++;
            }

            return (childrenFailed == Children.Count) ? DecisionStatus.Failure : DecisionStatus.Running;
        }
    }
}