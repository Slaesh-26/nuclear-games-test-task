using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : ScriptableObject
{
    public abstract void Init(GridMap map);
    public abstract List<GridCell> GetPath(Vector2Int start, Vector2Int end);
}
