using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavePathfinder", menuName = "Pathfinder/WavePathfinder")]
public class WavePathfinder : Pathfinder
{
    private const int MOVE_COST = 1;
    private const int OBSTACLE_COST = -1;
    private const int UNEXPLORED_COST = -2;
    
    private GridMap map;
    private LinkedList<GridCell> activeCells;
    
    public override void Init(GridMap map)
    {
        this.map = map;
        activeCells = new LinkedList<GridCell>();
    }

    public override List<GridCell> GetPath(Vector2Int startPos, Vector2Int endPos)
    {
        map.TryGetCell(startPos, out GridCell start);
        map.TryGetCell(endPos, out GridCell end);
        
        foreach (GridCell cell in activeCells)
        {
            cell.WaveCost = UNEXPLORED_COST;
        }
        
        activeCells.Clear();
        start.WaveCost = 0;

        if (SpreadWave(start, end))
        {
            return BacktracePath(start, end);
        }

        return null;
    }

    private bool SpreadWave(GridCell start, GridCell end)
    {
        int waveRadius = 0;
        int cellCounter = 0;

        if (!start.IsWalkable || !end.IsWalkable)
        {
            Debug.LogWarning("Path does not exist");
            return false;
        }

        activeCells.AddLast(start);

        while (true)
        {
            int cellsInWaveFrontWithValidNeighbours = 0;
            
            for (int i = -waveRadius; i < waveRadius + 1; i++)
            {
                for (int j = -waveRadius; j < waveRadius + 1; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) > waveRadius) continue;
                    Vector2Int pos = start.MapPos + new Vector2Int(i, j);
                    
                    if (!map.TryGetCell(pos, out GridCell cell)) continue;
                    if (cell.WaveCost != waveRadius) continue;

                    int validNeighboursCount = 0;
                    
                    foreach (GridCell neighbour in GetNeighbours(cell.MapPos))
                    {
                        if (neighbour.WaveCost >= 0 || neighbour.WaveCost == OBSTACLE_COST)
                        {
                            continue;
                        }
                                
                        if (neighbour.IsWalkable)
                        {
                            neighbour.WaveCost = cell.WaveCost + MOVE_COST;
                            activeCells.AddLast(neighbour);
                            validNeighboursCount++;
                        }
                        else
                        {
                            neighbour.WaveCost = OBSTACLE_COST;
                        }
                                
                        cellCounter++;

                        if (neighbour == end)
                        {
                            return true;
                        }
                    }

                    if (validNeighboursCount > 0)
                    {
                        cellsInWaveFrontWithValidNeighbours++;
                    }
                }
            }

            if (cellsInWaveFrontWithValidNeighbours == 0)
            {
                break;
            }
            waveRadius++;
        }
        
        Debug.LogWarning($"Path does not exist, cells explored: {cellCounter}");
        return false;
    }

    private List<GridCell> BacktracePath(GridCell start, GridCell end)
    {
        GridCell current = end;
        List<GridCell> path = new List<GridCell>() { end };

        while (current != start)
        {
            GridCell cellWithMinCost = null;

            if (current == null)
            {
                break;
            }

            foreach (GridCell neighbour in GetNeighbours(current.MapPos))
            {
                if (path.Contains(neighbour))
                {
                    continue;
                }
                
                if (neighbour.WaveCost < 0)
                {
                    continue;
                }

                if (cellWithMinCost == null)
                {
                    cellWithMinCost = neighbour;
                }
                
                if (neighbour.WaveCost < cellWithMinCost.WaveCost ||
                   (neighbour.WaveCost == cellWithMinCost.WaveCost && 
                    GetDistance(neighbour, start) < GetDistance(cellWithMinCost, start)))
                {
                    cellWithMinCost = neighbour;
                }
            }

            if (cellWithMinCost == null)
            {
                Debug.LogWarning("Path does not exist");
                return null;
            }

            current = cellWithMinCost;
            path.Add(current);
        }
        
        path.Reverse();
        return path;
    }

    private float GetDistance(GridCell cell1, GridCell cell2)
    {
        return Vector2Int.Distance(cell1.MapPos, cell2.MapPos);
    }
    
    private IEnumerable<GridCell> GetNeighbours(Vector2Int cellPos)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i * i + j * j != 1) continue;

                Vector2Int newPos = new Vector2Int(cellPos.x + i, cellPos.y + j);
                if (map.TryGetCell(newPos, out GridCell neighbour))
                {
                    yield return neighbour;
                }
            }
        }
    }
}
