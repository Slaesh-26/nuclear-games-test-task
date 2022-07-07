using System;
using UnityEditor;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public GridInteractor Interactor => gridInteractor;
    
    [SerializeField] [Min(0)] private Vector2Int size;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private GridInteractor gridInteractor;
    [SerializeField] private GridCellVisualsSpawner gridCellSpawner;

    private GridCell[,] cells;

    public void Init()
    {
        cells = new GridCell[size.y, size.x];

        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                Vector2Int cellMapPos = new Vector2Int(i, j);
                Vector3 cellWorldPos = GetCellWorldPos(cellMapPos);
                
                GridCellVisuals visuals = gridCellSpawner.SpawnCell(cellWorldPos);
                GridCell gridCell = new GridCell(cellMapPos, cellWorldPos, visuals);
                gridCell.SetAsWalkable();

                cells[i, j] = gridCell;
            }
        }
        
        gridInteractor.Init(transform.position.y, this);
    }

    public void SetObstacle(Vector2Int cellPos)
    {
        if (TryGetCell(cellPos, out GridCell cell))
        {
            cell.SetAsObstacle();
        }
    }

    public bool TryGetCell(Vector2Int mapPos, out GridCell cell)
    {
        try
        {
            cell = cells[mapPos.x, mapPos.y];
            return true;
        }
        catch (IndexOutOfRangeException e)
        {
            cell = null;
            return false;
        }
    }

    public CellData GetCellData(Vector3 worldPos)
    {
        Vector2Int mapPos = GetMapFromWorldPos(worldPos);
        return cells[mapPos.x, mapPos.y].GetCellData();
    }

    private Vector2Int GetMapFromWorldPos(Vector3 worldPos)
    {
        Vector2Int mapPos = new Vector2Int((int) (worldPos.z / cellSize), 
                                           (int) (worldPos.x / cellSize));
        mapPos = new Vector2Int(Mathf.Clamp(mapPos.x, 0, size.x - 1),
                                Mathf.Clamp(mapPos.y, 0, size.y - 1));
        
        return mapPos;
    }

    private Vector3 GetCellWorldPos(Vector2Int mapPos)
    {
        Vector3 pos = transform.forward * (cellSize * (mapPos.x + 0.5f)) +
                      transform.right * (cellSize * (mapPos.y + 0.5f)) +
                      transform.position;
        return pos;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                Vector2Int cellMapPos = new Vector2Int(i, j);
                Vector3 cellWorldPos = GetCellWorldPos(cellMapPos);
                Gizmos.DrawWireCube(cellWorldPos, new Vector3(cellSize, 0, cellSize));
            }
        }
    }
    
#endif
}
