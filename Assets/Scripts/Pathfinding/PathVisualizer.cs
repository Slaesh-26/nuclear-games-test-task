using System.Collections.Generic;
using UnityEngine;

public abstract class PathVisualizer : MonoBehaviour
{
    public abstract void DrawPath(List<GridCell> path);
}
