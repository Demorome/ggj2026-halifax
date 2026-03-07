using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider))]
    public class GroundedStateComponent : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        //private Collider collider;

        void Start()
        {
            //collider =  GetComponent<Collider>();
        }

        void Update()
        {
        }

        // If you exit, as in leave the collision,
        // it checks if the object you left collision with was environment,
        // and if so you can't jump again.
        public void OnTriggerExit(Collider collObj)
        {
            if (collObj.tag == "environment")
            {
                IsGrounded = false;
            }
        }

        public void OnTriggerEnter(Collider collObj)
        {
            if (collObj.tag == "environment")
            {
                IsGrounded = true;
            }
        }
    }
}