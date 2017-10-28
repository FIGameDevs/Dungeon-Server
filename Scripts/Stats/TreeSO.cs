using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Stat Tree")]
public class TreeSO : ScriptableObject
{
    [System.Serializable]
    public struct SerNode {
        public string name;
        public string[] parents;
        public Vector2 pos;
    }

    public SerNode[] nodes;

}
