using GameDevUtils.ObjectManagement;
using GameDevUtils.Resource;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static GameDevUtils.AI.DecisionTree.DecisionTreeNode;

namespace GameDevUtils.AI.DecisionTree
{
    public class DecisionTreeBuilder : Singleton<DecisionTreeBuilder>
    {
        /// <summary>
        /// Append a node to a parent node.
        /// This method can be used recursively to build an entire tree, by providing it with the root node.
        /// </summary>
        /// <param name="xmlTag">The node's extracted XML tag</param>
        /// <param name="behaviour">A script that contains the data needed to build the tree</param>
        /// <param name="parent">The node's parent (pass null for the root node)</param>
        /// <returns>The node created from the XML tag.</returns>
        private DecisionTreeNode AppendTreeNode(XElement xmlTag, DecisionTreeBehaviour behaviour, DecisionTreeNode parent = null) {
            DecisionTreeNode node = CreateNode(xmlTag, behaviour);
            IEnumerable<XElement> children = xmlTag.Elements();
            foreach (XElement childXml in children) AppendTreeNode(childXml, behaviour, node);

            if (parent != null) parent.AppendChildNode(node);
            return node;
        }

        /// <summary>
        /// Create a decision tree node.
        /// </summary>
        /// <param name="xmlTag">The XML tag from which to create the node</param>
        /// <param name="behaviour">A script that contains the data needed to build the tree</param>
        /// <returns>The node created from the XML tag.</returns>
        private DecisionTreeNode CreateNode(XElement xmlTag, DecisionTreeBehaviour behaviour) {
            DecisionTreeNode node = null;
            XMLUtils.TryGetStringAttr(xmlTag, "name", out string nameAttr);
            XMLUtils.TryGetStringAttr(xmlTag, "method", out string methodAttr);
            XMLUtils.TryGetNumberAttr(xmlTag, "weight", out float weightAttr);

            switch (xmlTag.Name.LocalName) {
                case "Root":
                    node = new DecisionTreeRoot(nameAttr);
                    break;

                case "If":
                case "WeightedIf":
                case "And":
                case "Sequence":
                case "WeightedSequence":
                    node = new SequenceNode(nameAttr, weightAttr);
                    break;

                case "Or":
                    node = new OrNode(nameAttr, weightAttr);
                    break;

                case "Not":
                    node = new InverterNode(nameAttr, weightAttr);
                    break;

                case "Condition":
                case "Action":
                case "WeightedAction":
                    node = new LeafNode(nameAttr, GetImplementation(behaviour, methodAttr), weightAttr);
                    break;

                case "Selector":
                case "WeightedSelector":
                    node = new WeightedSelectorNode(nameAttr, weightAttr);
                    break;

                case "RandomSelector":
                case "WeightedRandomSelector":
                    node = new RandomSelectorNode(nameAttr, weightAttr);
                    break;
            }

            return node;
        }

        /// <param name="behaviour">The script from which to pull the method implemetaion</param>
        /// <param name="methodName">The method's name</param>
        /// <returns>The method's implementation, or a default function that returns a failed status if the method does not exist.</returns>
        private LeafImplemetation GetImplementation(DecisionTreeBehaviour behaviour, string methodName) {
            LeafImplemetation method;

            try {
                method = Delegate.CreateDelegate(typeof(LeafImplemetation), behaviour, methodName) as LeafImplemetation;
            }
            catch (MissingMethodException ex) {
                method = delegate { return DecisionStatus.Failure; };
                throw ex;
            }

            return method;
        }

        /// <summary>
        /// Create a decision tree.
        /// </summary>
        /// <param name="behaviour">A script that contains the data needed to build the tree</param>
        /// <returns>The decision tree's root node</returns>
        public DecisionTreeRoot CreateTree(DecisionTreeBehaviour behaviour) {
            TextAsset xmlAsset = Resources.Load<TextAsset>(behaviour.XMLTreeName);
            XDocument doc = XDocument.Parse(xmlAsset.text);

            XElement root = doc.Element("Root");
            return AppendTreeNode(root, behaviour) as DecisionTreeRoot;
        }
    }
}