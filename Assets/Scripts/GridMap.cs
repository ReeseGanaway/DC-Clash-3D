using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    Node[,] gridMap;
    public int width = 25;
    public int length = 25;
    [SerializeField] float cellSize = 1f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask terrainLayer;

    private void Awake()
    {
        GenerateGrid();
    }

    // For visualizing the points on the grid
    private void OnDrawGizmos()
    {
        if (gridMap == null)
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Vector3 pos = GetWorldPosition(x, y);
                    Gizmos.DrawCube(pos, Vector3.one / 4);
                }
            }
        }
        else
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Vector3 pos = GetWorldPosition(x, y, true);
                    Gizmos.color = gridMap[x, y].walkable ? Color.green : Color.red;
                    Gizmos.DrawCube(pos, Vector3.one / 4);
                }
            }
        }
    }

    private void GenerateGrid()
    {
        gridMap = new Node[length, width];

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                gridMap[x, y] = new Node();
            }
        }
        CalculateElevation();
        CheckWalkableTerrain();
    }

    //Checks if a node is able to be walked on
    private void CheckWalkableTerrain()
    {
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y, true);
                bool walkable = !Physics.CheckBox(worldPosition, Vector3.one / 2 * cellSize, Quaternion.identity, obstacleLayer);
                gridMap[x, y].walkable = walkable;
            }
        }
    }

    public bool CheckWalkable(int x, int y)
    {
        return gridMap[x, y].walkable;
    }

    //gets position on the map, including elevation
    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        return new Vector3(x * cellSize, elevation == true ? gridMap[x, y].elevation : 0f, y * cellSize);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        Vector2Int positionOnGrid = new Vector2Int((int)(worldPosition.x / cellSize), (int)(worldPosition.z / cellSize));
        return positionOnGrid;
    }

    public void PlaceObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        if (CheckBoundary(positionOnGrid))
        {
            gridMap[positionOnGrid.x, positionOnGrid.y].gridObject = gridObject;
        }

    }

    //if click is within the boundaries of the grid
    public bool CheckBoundary(Vector2Int positionOnGrid)
    {
        if (positionOnGrid.x < 0 || positionOnGrid.x > length)
        {
            return false;
        }
        if (positionOnGrid.y < 0 || positionOnGrid.y > width)
        {
            return false;
        }
        return true;
    }

    //checking if we are within boundaries for pathfinding
    internal bool CheckBoundary(int x, int y)
    {
        if (x < 0 || x > length)
        {
            return false;
        }
        if (y < 0 || y > width)
        {
            return false;
        }
        return true;
    }

    internal GridObject GetPlacedObject(Vector2Int gridPosition)
    {
        if (CheckBoundary(gridPosition))
        {
            GridObject gridObject = gridMap[gridPosition.x, gridPosition.y].gridObject;
            return gridObject;
        }
        return null;

    }

    //uses a ray to calculate the elevation of each node
    private void CalculateElevation()
    {
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                Ray ray = new Ray(GetWorldPosition(x, y) + Vector3.up * 100, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayer))
                {
                    Debug.DrawRay(GetWorldPosition(x, y) + Vector3.up * 100, Vector3.down, Color.blue);
                    gridMap[x, y].elevation = hit.point.y;
                }
            }
        }
    }
}
