using System.Collections.Generic;
using UnityEngine;

public interface IAStarNode<TNode> : IHeapItem<TNode>
{
    Vector2 Position { get; }
    bool Walkable { get; }
    int GCost { get; set; }
    int HCost { get; set; }
    TNode Parent { get; set; }
    IEnumerable<TNode> GetNeighbours();
}
