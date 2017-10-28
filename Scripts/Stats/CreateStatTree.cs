using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateStatTree : MonoBehaviour
{
    [Serializable]
    struct TreeNode
    {
        public string Node;
        public string[] Parents;
    }

    [SerializeField]
    TreeNode[] nodes;

    void Awake()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            Stats.PerkInheritance.AddChild(nodes[i].Node, nodes[i].Parents);
        }
    }
}
