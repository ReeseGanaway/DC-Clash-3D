using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    [SerializeField] GridMap targetGrid;
    [SerializeField] LayerMask terrainLayer;

    Pathfinding pathfinding;
    Vector2Int currentPosition = new Vector2Int();
    List<PathNode> path;

    private void Start()
    {
        pathfinding = targetGrid.GetComponent<Pathfinding>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayer))
            {
                Vector2Int gridPosition = targetGrid.GetGridPosition(hit.point);

                if (gridPosition.x >= 0 && gridPosition.x < targetGrid.length && gridPosition.y >= 0 && gridPosition.y < targetGrid.width)
                {
                    Debug.Log("Current y " + currentPosition + " grid width " + targetGrid.width);
                    path = pathfinding.FindPath(currentPosition.x, currentPosition.y, gridPosition.x, gridPosition.y);
                    currentPosition = gridPosition;
                }

                Debug.Log(currentPosition.x + " " + currentPosition.y);



                /*
                GridObject gridObject = targetGrid.GetPlacedObject(gridPosition);
                Debug.Log(gridObject);
                if (gridObject == null)
                {
                    Debug.Log("x=" + gridPosition.x + " y=" + gridPosition.y + " is empty");
                }
                else
                {
                    Debug.Log(gridObject.GetComponent<Character>().unitName + " is at x=" + gridPosition.x + " y=" + gridPosition.y);
                }*/
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (path == null) return;
        if (path.Count == 0) return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(targetGrid.GetWorldPosition(path[i].xPos, path[i].yPos, true), targetGrid.GetWorldPosition(path[i + 1].xPos, path[i + 1].yPos, true));
        }
    }
}
