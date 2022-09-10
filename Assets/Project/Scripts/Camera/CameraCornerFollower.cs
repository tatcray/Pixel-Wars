using Dependencies;
using UnityEngine;

public class CameraCornerFollower
{
    private Vector3 defaultPosition;
    private GameObject cameraCorner;
    
    public CameraCornerFollower(CameraDependencies cameraDependencies)
    {
        cameraCorner = cameraDependencies.cameraCorner;
    }

    public void SaveCurrentPositionAsDefault()
    {
        defaultPosition = cameraCorner.transform.position;
    }

    public void TrySetNewCorner(Vector3 newPosition)
    {
        Vector3 cornerPosition = cameraCorner.transform.position;

        if (newPosition.x > cornerPosition.x)
            cornerPosition.x = newPosition.x;
        if (newPosition.y > cornerPosition.y)
            cornerPosition.y = newPosition.y;

        if (cornerPosition != cameraCorner.transform.position)
            cameraCorner.transform.position = cornerPosition;
    }

    public void ResetToDefaultPosition()
    {
        cameraCorner.transform.position = defaultPosition;
    }
}