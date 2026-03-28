using Components;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Determines if the camera stays still or follows a target.
    /// </summary>
    public bool isCameraStatic = false;

    /// <summary>
    /// Unused if camera is static.
    /// </summary>
    private CameraTargetComponent cameraTargetComponent;
    private GameObject CameraTarget => cameraTargetComponent
        ? cameraTargetComponent.gameObject
        : null;

    /// <summary>
    /// Save only the initial Y offset,
    /// AKA the distance from the camera from a bird's eye view.
    /// </summary>
    private float cameraPosYOffset;

    void Start()
    {
        if (isCameraStatic)
        {
            return;
        }

        cameraTargetComponent = (CameraTargetComponent)FindAnyObjectByType(
            typeof(CameraTargetComponent)
        );

        if (CameraTarget)
        {
            cameraPosYOffset = transform.position.y
                               - CameraTarget.transform.position.y;
        }
        else
        {
            Debug.Log("CameraTarget object not found.");
            cameraPosYOffset = transform.position.y;
        }
    }

    void LateUpdate()
    {
        if (isCameraStatic)
        {
            return;
        }

        // Try to find a new camera target if needed.
        if (!cameraTargetComponent || !cameraTargetComponent.gameObject.activeInHierarchy)
        {
            cameraTargetComponent = (CameraTargetComponent)FindAnyObjectByType(
                typeof(CameraTargetComponent)
            );
        }

        if (cameraTargetComponent)
        {
            float cameraZoomOutLevel;
            if (cameraTargetComponent.gameObject.TryGetComponent(
                    typeof(MassComponent),
                    out Component massComponent))
            {
                // Zoom out more if we're controlling something bigger.
                var sizePercent = ((MassComponent)massComponent).MassPercent;
                float minCameraZoom = 0.5f;

                cameraZoomOutLevel = minCameraZoom + sizePercent;
            }
            else
            {
                cameraZoomOutLevel = 1f;
            }

            transform.position = cameraTargetComponent.transform.position
                                 + new Vector3(0, cameraPosYOffset * cameraZoomOutLevel, 0);
        }
        else
        {
            Debug.Log("CameraTarget object not found.");
        }
    }
}