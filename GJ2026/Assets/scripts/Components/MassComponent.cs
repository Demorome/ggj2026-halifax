using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class MassComponent : MonoBehaviour
    {
        public float mass;

        // TODO: Implement knocking objects back based on mass!
    }
}