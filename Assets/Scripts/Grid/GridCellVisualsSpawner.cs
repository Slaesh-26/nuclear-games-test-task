using UnityEngine;

public class GridCellVisualsSpawner : MonoBehaviour
{
    [SerializeField] private GridCellVisuals prefab;
    
    public GridCellVisuals SpawnCell(Vector3 worldPos)
    {
        return Instantiate(prefab, worldPos, prefab.transform.rotation, transform);
    }
}
