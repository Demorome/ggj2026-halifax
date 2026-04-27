using UnityEngine;

namespace Components
{
    public class MoveSpeedComponent : MonoBehaviour
    {
        public bool canMove = true;
        public float baseSpeed = 10f;
        public float Speed => canMove ? baseSpeed : 0.0f;
    }
}