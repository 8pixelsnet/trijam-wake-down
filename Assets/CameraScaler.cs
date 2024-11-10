using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField]GameObject targetObject;
    [SerializeField] float padding = 1.1f; // Extra space around the object, 1.0 = exact fit
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        FitCamera();
    }

    void FitCamera()
    {
        Bounds objectBounds = targetObject.GetComponent<Renderer>().bounds;
        float objectWidth = objectBounds.size.x;
        float objectHeight = objectBounds.size.y;
        
        float screenAspect = Screen.width / (float)Screen.height;
        float targetAspect = objectWidth / objectHeight;

        if (screenAspect >= targetAspect)
        {
            cam.orthographicSize = objectHeight / 2 * padding;
        }
        else
        {
            cam.orthographicSize = objectWidth / 2 / screenAspect * padding;
        }
    }
}
