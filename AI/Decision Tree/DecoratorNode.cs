using JetBrains.Annotations;

namespace GameDevUtils.AI.DecisionTree
{
    public abstract class DecoratorNode : DecisionTreeNode
    {
        /// <inheritdoc/>
        public DecoratorNode(string name, float weight = 1) : base(name, weight) {}

        /// <inheritdoc/>
        protected override DecisionStatus ProcessNode() {
            if (Children.Count > 0 && Children[0] != null) return ProcessNode(Children[0]);
            else return DecisionStatus.Failure;
        }

        /// <see cref="Process"/>
        /// <param name="child">The first child node</param>
        protected abstract DecisionStatus ProcessNode([NotNull] DecisionTreeNode child);
    }
}