using System.Collections.Generic;
using UnityEngine;

public class DefaultPathVisualizer : PathVisualizer
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lineHeightAboveGrid = 0.1f;
    
    public override void DrawPath(List<GridCell> path)
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i].WorldPos + Vector3.up * lineHeightAboveGrid);
        }
    }
}
