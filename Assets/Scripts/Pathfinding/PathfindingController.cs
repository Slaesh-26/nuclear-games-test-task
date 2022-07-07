using System.Collections.Generic;
using UnityEngine;

public class PathfindingController : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private PathVisualizer pathVisualizer;
    [SerializeField] private Pathfinder pathfinder;
    
    private GridInteractor gridInteractor;
    private GridMap gridMap;
    private CellData previousClickedCell;

    public void Init(GridMap gridMap, GridInteractor gridInteractor)
    {
        this.gridMap = gridMap;
        this.gridInteractor = gridInteractor;
        pathfinder.Init(gridMap);

        inputController.choosePathPressed += OnChoosePath;
        inputController.placeObstaclePressed += OnPlaceObstacle;
    }

    private void FindAndDrawPath(Vector2Int startPos, Vector2Int endPos)
    {
        List<GridCell> path = pathfinder.GetPath(startPos, endPos);
        if (path != null)
        {
            pathVisualizer.DrawPath(path);
        }
    }

    private void OnDestroy()
    {
        inputController.choosePathPressed -= OnChoosePath;
        inputController.placeObstaclePressed -= OnPlaceObstacle;
    }

    private void OnChoosePath()
    {
        SetPath(gridInteractor.SelectedCell);
    }

    private void OnPlaceObstacle()
    {
        gridMap.SetSelectedCellObstacle();
    }

    private void SetPath(CellData data)
    {
        if (previousClickedCell.Equals(CellData.None))
        {
            previousClickedCell = data;
        }
        else
        {
            FindAndDrawPath(previousClickedCell.mapPos, data.mapPos);
            previousClickedCell = CellData.None;
        }
    }
}
