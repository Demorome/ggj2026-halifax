using UnityEngine;

namespace Components
{
    public class CameraTargetComponent : MonoBehaviour
    {
        //private CameraController cameraController;

        void Start()
        {
            //cameraController = FindAnyObjectByType<CameraController>();
        }

        public void ChangeCameraTarget(GameObject target)
        {
            target.AddComponent<CameraTargetComponent>();
            Destroy(this);
        }
    }
}