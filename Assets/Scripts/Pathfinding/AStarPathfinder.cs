using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AStarPathfinder", menuName = "Pathfinder/AStarPathfinder")]
public class AStarPathfinder : Pathfinder
{
    private GridMap map;
    private List<GridCell> openCells;
    private HashSet<GridCell> closedCells;

    private const int FORWARD_COST = 10;
    private const int DIAGONAL_COST = 14;

    public override void Init(GridMap map)
    {
        this.map = map;
        openCells = new List<GridCell>();
        closedCells = new HashSet<GridCell>();
    }

    public override List<GridCell> GetPath(Vector2Int startCellPos, Vector2Int endCellPos)
    {
        map.TryGetCell(startCellPos, out GridCell startCell);
        map.TryGetCell(endCellPos, out GridCell endCell);
        
        if (!startCell.IsWalkable || !endCell.IsWalkable)
        {
            Debug.LogWarning("Path does not exist");
            return null;
        }

        foreach (GridCell cell in openCells)
        {
            cell.SetDefaultCostValues();
            cell.PreviousCell = null;
        }
        
        foreach (GridCell cell in closedCells)
        {
            cell.SetDefaultCostValues();
            cell.PreviousCell = null;
        }

        openCells.Clear();
        closedCells.Clear();
        openCells.Add(startCell);

        startCell.GCost = 0;
        startCell.HCost = GetDistanceCost(startCell.MapPos, endCell.MapPos);

        while (openCells.Count > 0)
        {
            GridCell current = GetCellWithSmallestFCost(openCells);
            if (current == endCell)
            {
                return GetPathFromEndCell(endCell);
            }

            openCells.Remove(current);
            closedCells.Add(current);

            foreach (GridCell neighbour in GetNeighbours(current))
            {
                if (closedCells.Contains(neighbour)) continue;
                if (!neighbour.IsWalkable)
                {
                    closedCells.Add(neighbour);
                    continue;
                }
                
                int newGCost = current.GCost + GetDistanceCost(current.MapPos, neighbour.MapPos);

                if (newGCost < neighbour.GCost)
                {
                    neighbour.PreviousCell = current;
                    neighbour.GCost = newGCost;
                    neighbour.HCost = GetDistanceCost(neighbour.MapPos, endCell.MapPos);
                    
                    if (!openCells.Contains(neighbour))
                    {
                        openCells.Add(neighbour);
                    }
                }
            }
        }
        
        Debug.LogWarning("Path does not exist");
        return null;
    }
    
    private IEnumerable<GridCell> GetNeighbours(GridCell cell)
    {
        Vector2Int mapPos = cell.MapPos;
        
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;
                Vector2Int neighbourPos = new Vector2Int(mapPos.x + i, mapPos.y + j);
                
                if (map.TryGetCell(neighbourPos, out GridCell neighbour))
                {
                    yield return neighbour;    
                }
            }
        }
    }

    private List<GridCell> GetPathFromEndCell(GridCell endCell)
    {
        List<GridCell> path = new List<GridCell>() { endCell };
        GridCell current = endCell;

        while (current.PreviousCell != null)
        {
            path.Add(current.PreviousCell);
            current = current.PreviousCell;
        }

        path.Reverse();
        return path;
    }

    private int GetDistanceCost(Vector2Int start, Vector2Int end)
    {
        int xDist = Mathf.Abs(start.x - end.x);
        int yDist = Mathf.Abs(start.y - end.y);
        int left = Mathf.Abs(xDist - yDist);
        int result = Mathf.Min(xDist, yDist) * DIAGONAL_COST + FORWARD_COST * left;

        return result;
    }

    private GridCell GetCellWithSmallestFCost(List<GridCell> cells)
    {
        if (cells == null || cells.Count == 0)
        {
            Debug.LogError("Cells list is null or empty");
            return null;
        }
        
        GridCell smallestFCost = cells[0];
        for (int i = 1; i < cells.Count; i++)
        {
            if (cells[i].FCost < smallestFCost.FCost)
            {
                smallestFCost = cells[i];
            }
        }

        return smallestFCost;
    }
}
