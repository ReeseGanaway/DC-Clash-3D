using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for a path node for each node in the grid
public class PathNode
{
    public int xPos;
    public int yPos;

    //cost of travel from starting node to this node
    public float gValue;

    //cost of travel from this node to the end node
    public float hValue;

    //the sum of the g and h values, total cost of the node
    public float fValue
    {
        get { return gValue + hValue; }
    }

    public PathNode parentNode;

    public PathNode(int xCoord, int yCoord)
    {
        xPos = xCoord;
        yPos = yCoord;
    }
}

[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour
{
    GridMap gridMap;
    PathNode[,] path;
    private void Start()
    {
        Init();
    }

    //initialize a grid of path nodes
    private void Init()
    {
        if (gridMap == null) { gridMap = GetComponent<GridMap>(); }
        path = new PathNode[gridMap.length, gridMap.width];
        for (int x = 0; x < gridMap.length; x++)
        {
            for (int y = 0; y < gridMap.width; y++)
            {
                path[x, y] = new PathNode(x, y);
            }
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        //save our start and end points for reference
        PathNode startNode = path[startX, startY];
        PathNode endNode = path[endX, endY];

        //openList contains nodes that are still open for exploration/havent been traveled
        List<PathNode> openList = new List<PathNode>();
        //closedList contains nodes that havent been explored or tested for travel yet
        List<PathNode> closedList = new List<PathNode>();

        //add the starting point to the openList
        openList.Add(startNode);

        //while openList isnt empty
        while (openList.Count > 0)
        {
            PathNode currentNode = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                //
                if (currentNode.fValue > openList[i].fValue)
                {
                    currentNode = openList[i];
                }
                if (currentNode.fValue == openList[i].fValue && currentNode.hValue > openList[i].hValue)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            List<PathNode> neighbors = new List<PathNode>();

            //add the adjacent nodes to neighbors
            if (currentNode.xPos - 1 >= 0) neighbors.Add(path[currentNode.xPos - 1, currentNode.yPos]);
            if (currentNode.xPos + 1 < gridMap.length) neighbors.Add(path[currentNode.xPos + 1, currentNode.yPos]);
            if (currentNode.yPos - 1 >= 0) neighbors.Add(path[currentNode.xPos, currentNode.yPos - 1]);
            if (currentNode.yPos + 1 < gridMap.width) neighbors.Add(path[currentNode.xPos, currentNode.yPos + 1]);

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (closedList.Contains(neighbors[i])) continue;
                if (!gridMap.CheckWalkable(neighbors[i].xPos, neighbors[i].yPos)) continue;

                float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbors[i]);

                if (!openList.Contains(neighbors[i]) || movementCost < neighbors[i].gValue)
                {
                    neighbors[i].gValue = movementCost;
                    neighbors[i].hValue = CalculateDistance(neighbors[i], endNode);
                    neighbors[i].parentNode = currentNode;

                    if (!openList.Contains(neighbors[i])) openList.Add(neighbors[i]);

                }
            }

        }
        return null;
    }

    //get the distance between 2 nodes
    private int CalculateDistance(PathNode start, PathNode end)
    {
        int distX = Mathf.Abs(start.xPos - end.xPos);
        int distY = Mathf.Abs(start.yPos - end.yPos);

        if (distX > distY) return (14 * distY + 10 * (distX - distY));
        return (14 * distX + 10 * (distY - distX));
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> retrace = new List<PathNode>();

        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            retrace.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        retrace.Reverse();
        return retrace;
    }
}
