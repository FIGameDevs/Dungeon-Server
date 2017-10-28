using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class PerkNode
    {
        string name;
        public PerkNode[] Parents { get; private set; }

        public PerkNode(string n, PerkNode[] parents)
        {
            name = n;
            Parents = parents;
        }

        public override string ToString()
        {
            return name + " perk node. Has " + Parents?.Length + " parents.";
        }
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
    //There should be a class that adds all weapon/magic categories at the start of the game. Then every weapon/spell can add itself in its class.
    public static class PerkInheritance
    {

        static Dictionary<string, PerkNode> tree = new Dictionary<string, PerkNode>();

        public static PerkNode AddChild(string childName, params string[] parents)
        {
            PerkNode node = null;
            if (tree.TryGetValue(childName, out node))
            {
                Utils.EditorLog("Error: This child is already in the tree.");
                return node;
            }
            else if (parents != null)
            {
                List<PerkNode> nodes = new List<PerkNode>(parents.Length);
                for (int i = 0; i < parents.Length; i++)
                {
                    PerkNode parent;
                    if (!tree.TryGetValue(parents[i], out parent))
                    {
                        Utils.EditorLog("Error: Perk parent " + parents[i] + " is not in the node tree. Did you misspell?");
                    }
                    else
                        nodes.Add(parent);
                }
                node = new PerkNode(childName, nodes.ToArray());
            }
            else
                node = new PerkNode(childName, null);

            tree.Add(childName, node);
            return node;
        }

        public static PerkNode GetNode(string nodeName)
        {
            PerkNode node;
            if (!tree.TryGetValue(nodeName, out node))
            {
                Utils.EditorLog("Error: Node '" + nodeName + "' not found in node tree.");
                return new PerkNode(nodeName, null);
            }
            return node;
        }
    }
}