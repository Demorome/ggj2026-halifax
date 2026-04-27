using UnityEngine;

namespace Components
{
    public class RotateSpeedComponent : MonoBehaviour
    {
        public bool canMove = true;
        public float baseSpeed = 150f;
        public float Speed => canMove ? baseSpeed : 0.0f;
    }
}