using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public event Action placeObstaclePressed;
    public event Action choosePathPressed;
    
    private KeyCode placeObstacle = KeyCode.Mouse2;
    private KeyCode choosePath = KeyCode.Mouse0;
    
    private void Update()
    {
        if (Input.GetKey(placeObstacle))
        {
            placeObstaclePressed?.Invoke();
        }

        if (Input.GetKeyDown(choosePath))
        {
            choosePathPressed?.Invoke();
        }
    }
}
