using UnityEngine;

public class GridInteractor : MonoBehaviour
{
    public CellData CurrentCell { get; private set; }
    
    [SerializeField] private GameObject marker;
    
    private bool isOrthographic;
    private GridMap grid;
    private Camera mainCamera;
    private CellData lastSelectedCell;
    private float gridMapWorldHeight;
    
    public void Init(float gridMapWorldHeight, GridMap grid)
    {
        mainCamera = Camera.main;
        isOrthographic = mainCamera.orthographic;
        this.grid = grid;
        this.gridMapWorldHeight = gridMapWorldHeight;
    }

    private void Update()
    {
        Vector2 screenPos = Input.mousePosition;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1f));
        Vector3 mouseRay;
        
        if (isOrthographic)
        {
            mouseRay = mainCamera.transform.forward;
        }
        else
        {
            mouseRay = mousePos - mainCamera.transform.position;
        }
        
        float n = (-mousePos.y + gridMapWorldHeight) / mouseRay.y;

        Vector3 mouseGridWorldPos = mousePos + n * mouseRay;
        CellData currentSelectedCell = grid.GetCellData(mouseGridWorldPos);
        CurrentCell = currentSelectedCell;

        marker.transform.position = currentSelectedCell.worldPos;

        if (!lastSelectedCell.Equals(currentSelectedCell))
        {
            lastSelectedCell = currentSelectedCell;
        }
    }
}
