using System;
using System.Collections.Generic;
using UnityEngine;

public class AStar<TNode> where TNode : class, IAStarNode<TNode>
{
    private int maxSize;

    public AStar(int maxSize)
    {
        this.maxSize = maxSize;
    }

    public List<TNode> FindPath(TNode start, TNode end, Func<TNode, bool> validation)
    {
        Heap<TNode> open = new Heap<TNode>(maxSize);
        HashSet<TNode> closed = new HashSet<TNode>();

        open.Add(start);
        while (open.Count > 0)
        {
            TNode currentNode = open.RemoveFirst();
            closed.Add(currentNode);

            if (currentNode == end)
            {
                var path = RetracePath(start, end);

                foreach (var node in path)
                {
                    node.GCost = 0;
                    node.HCost = 0;
                    node.Parent = null;
                }

                return path;
            }

            foreach (var neighbour in currentNode.GetNeighbours())
            {
                if (closed.Contains(neighbour) || !validation(neighbour)) continue;

                int newMovementCostToNeighbour =
                    currentNode.GCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.GCost || !open.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, end);
                    neighbour.Parent = currentNode;

                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                }
            }
        }

        return null;
    }

    private List<TNode> RetracePath(TNode start, TNode end)
    {
        List<TNode> path = new List<TNode>();
        TNode currntNode = end;

        while (currntNode != start)
        {
            path.Add(currntNode);
            currntNode = currntNode.Parent;
        }

        path.Reverse();

        return path;
    }

    protected virtual int GetDistance(TNode nodeA, TNode nodeB)
    {
        int distanceX = (int)Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        int distanceY = (int)Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}