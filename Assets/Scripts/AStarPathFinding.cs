using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Node is the base class for path finding
    public Node parent = null;
    public int f = 0;
    public int g = 0;
    public int h = 0;
    public Vector3Int position = Vector3Int.zero;

    public Node(Vector3Int pos)
    {
        position = pos;
    }

    public Node() // default constructor
    {
        position = Vector3Int.zero;
    }

    public void SetParent(Node n) // set parent node
    {
        parent = n;
    }

    public void SetPosition(Vector3Int pos) // set position on grid
    {
        position = pos;
    }

    public void ComputeH(Vector3Int target) // compute heuristic from target position
    {
        h = (int)Mathf.Pow(Mathf.Abs(position.x - target.x), 2) + (int)Mathf.Pow(Mathf.Abs(position.z - target.z), 2);
    }

    public void ComputeG() // compute distance from parent position
    {
        g = parent.g + 1;
    }

    public void ComputeF() // compute node function value
    {
        f = g + h;
    }
}

public class AStarPathFinding : MonoBehaviour
{

    private List<Node> openList;
    private List<Node> closedList;

    public List<Vector3Int> PathFinding(List<Vector3Int> cells, Vector3Int start, Vector3Int target)
    {
        // Add the target to the cells list
        List<Vector3Int> cellsList = new List<Vector3Int>(cells);
        cellsList.Add(target);

        // Create the start node
        Node startNode = new Node(start);

        // Create the target node
        Node endNode = new Node(target);

        // Start of A* (star) Pathfinding (from Nicholas Swift blog: https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2)

        /* Initialize both open and closed list:
            let the openList equal empty list of nodes
            let the closedList equal empty list of nodes */
        openList = new List<Node>();
        closedList = new List<Node>();
        Node currentNode = startNode;

        // Add the start node: put the startNode on the openList(leave it's f at zero)
        openList.Add(startNode);
        // Loop until you find the end
        while (openList.Count != 0){
            // Get the current node: let the currentNode equal the node with the least f value
            currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].f < currentNode.f) currentNode = openList[i];
            }

            // remove the currentNode from the openList
            openList.Remove(currentNode);

            // add the currentNode to the closedList
            closedList.Add(currentNode);

            // Found the goal: if currentNode is the goal, Congratz! You've found the end! Backtrack to get path
            if (currentNode.position.Equals(target))
            {
                List<Vector3Int> path = new List<Vector3Int>();
                Node n = currentNode;
                while (!n.position.Equals(start))
                {
                    path.Add(n.position);
                    n = n.parent;
                }
                return path;
            }

            // Generate children: let the children of the currentNode equal the adjacent nodes
            List<Node> children = new List<Node>();
            Vector3Int ch = new Vector3Int();
            Node chNode = new Node();
            if(cellsList.Exists(c => c.Equals(currentNode.position + new Vector3Int(1, 0, 0))))
            {
                //if(FindObjectOfType<GridLocation>().GetComponent<GridLocation>().areNeighbours(currentNode.position, currentNode.position + new Vector3Int(1, 0, 0)) || target.Equals(currentNode.position + new Vector3Int(1, 0, 0)))
                if (!FindObjectOfType<GridLocation>().GetComponent<GridLocation>().hasWestWall(currentNode.position))
                {
                    ch = cellsList.Find(c => c.Equals(currentNode.position + new Vector3Int(1, 0, 0)));
                    chNode = new Node(ch);
                    children.Add(chNode);
                }                
            }

            if (cellsList.Exists(c => c.Equals(currentNode.position + new Vector3Int(-1, 0, 0))))
            {
                //if (FindObjectOfType<GridLocation>().GetComponent<GridLocation>().areNeighbours(currentNode.position, currentNode.position + new Vector3Int(-1, 0, 0)) || target.Equals(currentNode.position + new Vector3Int(-1, 0, 0)))
                if (!FindObjectOfType<GridLocation>().GetComponent<GridLocation>().hasEastWall(currentNode.position))
                {
                    ch = cellsList.Find(c => c.Equals(currentNode.position + new Vector3Int(-1, 0, 0)));
                    chNode = new Node(ch);
                    children.Add(chNode);
                }
            }

            if (cellsList.Exists(c => c.Equals(currentNode.position + new Vector3Int(0, 1, 0))))
            {
                //if (FindObjectOfType<GridLocation>().GetComponent<GridLocation>().areNeighbours(currentNode.position, currentNode.position + new Vector3Int(0, 1, 0)) || target.Equals(currentNode.position + new Vector3Int(0, 1, 0)))
                if (!FindObjectOfType<GridLocation>().GetComponent<GridLocation>().hasNorthWall(currentNode.position))
                {
                    ch = cellsList.Find(c => c.Equals(currentNode.position + new Vector3Int(0, 1, 0)));
                    chNode = new Node(ch);
                    children.Add(chNode);
                }
            }

            if (cellsList.Exists(c => c.Equals(currentNode.position + new Vector3Int(0, -1, 0))))
            {
                //if (FindObjectOfType<GridLocation>().GetComponent<GridLocation>().areNeighbours(currentNode.position, currentNode.position + new Vector3Int(0, -1, 0)) || target.Equals(currentNode.position + new Vector3Int(0, -1, 0)))
                if (!FindObjectOfType<GridLocation>().GetComponent<GridLocation>().hasSouthWall(currentNode.position))
                {
                    ch = cellsList.Find(c => c.Equals(currentNode.position + new Vector3Int(0, -1, 0)));
                    chNode = new Node(ch);
                    children.Add(chNode);
                }
            }

            // For each child in the children
            foreach (Node child in children)
            {
                // Child is on the closedList: if child is in the closedList, continue to beginning of for loop
                if (closedList.Exists(c => c.position.Equals(child.position)))
                {
                    continue;
                }

                // Child is already in openList: if child.position is in the openList's nodes positions and if the possible new child.g is higher than the openList node's g, continue to beginning of for loop
                if (openList.Exists(c => c.position.Equals(child.position)))
                {
                    if((currentNode.g + 1) > child.g)
                    continue;
                }

                // Create the f, g, and h values
                child.SetParent(currentNode);
                child.ComputeG();
                child.ComputeH(target);
                child.ComputeF();

                // Add the child to the openList
                openList.Add(child);
            }
        }
        return null;
    }
}
