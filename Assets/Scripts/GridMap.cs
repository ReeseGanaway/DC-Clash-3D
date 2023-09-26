using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    Node[,] gridMap;
    [SerializeField] int width = 25;
    [SerializeField] int length = 25;
    [SerializeField] float cellSize = 1f;
    [SerializeField] LayerMask obstacleLayer;

    private void Awake()
    {
        GenerateGrid();
    }

    private void OnDrawGizmos()
    {
        if (gridMap == null) { return; }
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Vector3 pos = GetWorldPosition(x, y);
                Gizmos.color = gridMap[x, y].walkable ? Color.green : Color.red;
                Gizmos.DrawCube(pos, Vector3.one / 4);
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
        CheckWalkableTerrain();
    }

    private void CheckWalkableTerrain()
    {
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                bool walkable = !Physics.CheckBox(worldPosition, Vector3.one / 2 * cellSize, Quaternion.identity, obstacleLayer);
                gridMap[x, y] = new Node();
                gridMap[x, y].walkable = walkable;
            }
        }
    }





    private Vector3 GetWorldPosition(int y, int x)
    {
        return new Vector3(transform.position.x + (x * cellSize), 0f, transform.position.z + (y * cellSize));
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        worldPosition -= transform.position;
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

    //if click is withing the boundaries of the grid
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

    internal GridObject GetPlacedObject(Vector2Int gridPosition)
    {
        if (CheckBoundary(gridPosition))
        {
            GridObject gridObject = gridMap[gridPosition.x, gridPosition.y].gridObject;
            return gridObject;
        }
        return null;

    }
}
