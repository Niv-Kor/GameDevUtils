using JetBrains.Annotations;

namespace GameDevUtils.AI.DecisionTree
{
    public class InverterNode : DecoratorNode
    {
        /// <inheritdoc/>
        public InverterNode(string name, float weight = 1) : base(name, weight) {}

        /// <inheritdoc/>
        protected override DecisionStatus ProcessNode([NotNull] DecisionTreeNode child) {
            switch (child.Process()) {
                case DecisionStatus.Success: return DecisionStatus.Failure;
                case DecisionStatus.Failure: return DecisionStatus.Success;
                default: return DecisionStatus.Running;
            }
        }
    }
}