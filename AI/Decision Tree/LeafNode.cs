namespace GameDevUtils.AI.DecisionTree
{
    public class LeafNode : DecisionTreeNode
    {
        #region Class Members
        private LeafImplemetation method;
        #endregion

        /// <inheritdoc/>
        public LeafNode(string name, LeafImplemetation method, float weight = 1) : base(name, weight) {
            this.method = method;
        }

        /// <inheritdoc/>
        protected override DecisionStatus ProcessNode() => method();
    }
}