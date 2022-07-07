using UnityEngine;

public class GridCell
{
    public Vector2Int MapPos { get; }
    public Vector3 WorldPos { get; }
    public bool IsWalkable { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public GridCell PreviousCell { get; set; }
    public int WaveCost { get; set; } = -2;

    private GridCellVisuals visuals;
    
    public GridCell(Vector2Int mapPos, Vector3 worldPos, GridCellVisuals visuals)
    {
        MapPos = mapPos;
        WorldPos = worldPos;
        IsWalkable = true;
        this.visuals = visuals;
        
        SetDefaultCostValues();
        PreviousCell = null;
    }
    
    public void SetAsObstacle()
    {
        IsWalkable = false;
        visuals.SetObstacleVisuals();
    }
    
    public void SetAsWalkable()
    {
        IsWalkable = true;
        visuals.SetWalkableVisuals();
    }

    public void SetDefaultCostValues()
    {
        GCost = int.MaxValue;
        HCost = 0;
    }

    public CellData GetCellData()
    {
        return new CellData()
        {
            mapPos = MapPos,
            worldPos = WorldPos,
        };
    }
}

public struct CellData
{
    public static CellData None => default(CellData);
    
    public Vector2Int mapPos;
    public Vector3 worldPos;
}