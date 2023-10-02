using GameDevUtils.EditorMode;
using GameDevUtils.ObjectManagement;
using System.Collections;

namespace GameDevUtils.AI.DecisionTree
{
    public class DecisionTreeProcessor : Singleton<DecisionTreeProcessor>
    {
        /// <summary>
        /// Process a decision tree once until it fails.
        /// </summary>
        /// <param name="tree">The decision tree to process</param>
        private IEnumerator ProcessTree(DecisionTreeRoot tree) {
            for (int i = 0; i < tree.Children.Count; i++) {
                DecisionStatus childStatus = DecisionStatus.Running;

                while (childStatus == DecisionStatus.Running) {
                    childStatus = tree.Children[i].Process();
                    yield return null;
                }

                if (childStatus == DecisionStatus.Failure) yield break;
            }

            tree.Reset();
        }

        /// <summary>
        /// Process a decision tree over and over again for each time it fails.
        /// </summary>
        /// <param name="tree">The decision tree to process</param>
        private IEnumerator LoopTree(DecisionTreeRoot tree) {
            while (true) {
                //ChanneledLogger.Log(LogChannel.AI, LogPriority.Info, $"---Running Decision Tree \"{tree.Name}\"---");
                yield return StartCoroutine(ProcessTree(tree));
            }
        }

        /// <summary>
        /// Process a decision tree.
        /// </summary>
        /// <param name="rootNode">The decision tree to process</param>
        public void Process(DecisionTreeRoot rootNode) {
            StartCoroutine(LoopTree(rootNode));
        }
    }
}