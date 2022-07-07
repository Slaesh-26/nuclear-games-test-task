using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellVisuals : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color obstacleColor = Color.red;
    [SerializeField] private Color defaultColor = Color.green;
    
    private MaterialPropertyBlock propertyBlock;
    private readonly int colorPropertyKey = Shader.PropertyToID("_Color");

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    public void SetObstacleVisuals()
    {
        propertyBlock.SetColor(colorPropertyKey, obstacleColor);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }
    
    public void SetWalkableVisuals()
    {
        propertyBlock.SetColor(colorPropertyKey, defaultColor);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }
}
