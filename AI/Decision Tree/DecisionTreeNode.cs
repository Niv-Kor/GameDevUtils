using GameDevUtils.EditorMode;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevUtils.AI.DecisionTree
{
    public abstract class DecisionTreeNode
    {
        public delegate DecisionStatus LeafImplemetation();

        #region Class Members
        protected int childIndex;
        #endregion

        #region Properties
        public string Name { get; protected set; }
        public float Weight { get; protected set; }
        public List<DecisionTreeNode> Children { get; protected set; }
        #endregion

        /// <param name="name">The node's name</param>
        public DecisionTreeNode(string name, float weight = 1) {
            this.Children = new List<DecisionTreeNode>();
            this.childIndex = 0;
            this.Name = name;
            this.Weight = Mathf.Clamp(weight, 0, 1);
        }

        /// <summary>
        /// Reset this node and all its children nodes (children first).
        /// </summary>
        public virtual void Reset() {
            foreach (DecisionTreeNode node in Children) node.Reset();
            childIndex = 0;
        }

        /// <param name="weight">The node's new weight value [0:1]</param>
        public virtual void SetWeight(float weight) => Weight = Mathf.Clamp(weight, 0, 1);

        /// <param name="name">The node's new name</param>
        public virtual void SetName(string name) => Name = name;

        /// <summary>
        /// Append a child node to this node.
        /// Each child is added to the right of the previous one.
        /// </summary>
        /// <param name="node">The child node to append</param>
        public virtual void AppendChildNode(DecisionTreeNode node) => Children.Add(node);
        
        /// <summary>
        /// Process this node's decision.
        /// </summary>
        public DecisionStatus Process() {
            DecisionStatus status = ProcessNode();

            string statusStr;
            switch (status) {
                case DecisionStatus.Success:
                    statusStr = "succeeded";
                    break;

                case DecisionStatus.Running:
                    statusStr = "is running";
                    break;

                case DecisionStatus.Failure:
                    statusStr = "failed";
                    break;

                default:
                    statusStr = "status unknown";
                    break;
            }

            string logMessage = $"Processing \"{Name}\". Process {statusStr}.";
            //ChanneledLogger.Log(LogChannel.AI, LogPriority.Info, logMessage);
            return status;
        }

        /// <summary>
        /// Process this node's decision.
        /// </summary>
        protected abstract DecisionStatus ProcessNode();
    }
}