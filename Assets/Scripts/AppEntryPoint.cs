using UnityEngine;

public class AppEntryPoint : MonoBehaviour
{
    [SerializeField] private GridMap gridMap;
    [SerializeField] private PathfindingController pathfindingController;
    
    private void Start()
    {
        gridMap.Init();
        pathfindingController.Init(gridMap, gridMap.Interactor);
    }
}
