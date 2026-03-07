using UnityEngine;
using UnityEngine.Serialization;

namespace Components
{
    /// <summary>
    /// Determines player controls, depending on the player's form/host. <br/>
    /// Some controls are universal/shared.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(RotateSpeedComponent))]
    [RequireComponent(typeof(MoveSpeedComponent))]
    public class ControlledByPlayerComponent : MonoBehaviour
    {
        public ActorType actorType;

        private Rigidbody _rb;
        private Collider _collider;
        private RotateSpeedComponent rotateSpeed;
        private MoveSpeedComponent moveSpeed;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _collider =  GetComponent<Collider>();

            rotateSpeed = GetComponent<RotateSpeedComponent>();
            moveSpeed = GetComponent<MoveSpeedComponent>();
        }

        void FixedUpdate()
        {
            if (GameManager.Instance.CurrentPlayingState != GameManager.PlayingState.Normal)
            {
                return;
            }

            //rotating based on mouse position
            //rb.rotation =  Quaternion.Euler(-Input.mousePosition.y , Input.mousePosition.x, 0);

            // Rotate facing direction based on horizontal axis input
            transform.Rotate(
                0,
                Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed.speed,
                0
            );

            // Move up/down based on vertical axis input, relative to our rotation.
            transform.Translate(
                0,
                0,
                Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed.speed,
                Space.Self
            );

            //on click do a swing
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("swing");

            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (GameManager.Instance.CurrentPlayingState != GameManager.PlayingState.Normal)
            {
                return;
            }

            // TODO: Implement!!
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Interacting!");

                if (other.CompareTag("interactArea"))
                {
                    //Debug.Log("in interact area");
                    // TODO: Add special logic for each different kind of interactArea!
                }
                else if (other.CompareTag("enemy"))
                {
                    //persistentPlayer.TryEquipEnemyAsHost(other.gameObject);
                }
            }
        }

        // Knock back method for if hit
        public void knockback(Quaternion rotation, float knockback)
        {
            Debug.Log("knockback");
            //send them back from where enemy is facing
            _rb.AddForce(rotation * Vector3.back * -knockback);
        }
    }
}