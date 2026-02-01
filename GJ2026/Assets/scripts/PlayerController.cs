using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

/// <summary>
/// NOTE: The player may switch from host to host, which may use different PlayerControllers!
/// This is used to enable multiple different collider shapes for different hosts.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody rb;
    public Collider collider;
    public PersistentPlayer persistentPlayer;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.CurrentPlayingState != GameManager.PlayingState.Normal)
        {
            return;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, Time.deltaTime * 250, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -Time.deltaTime * 250, 0);
        }
    }
    void FixedUpdate()
    {
        if (gameManager.CurrentPlayingState != GameManager.PlayingState.Normal)
        {
            return;
        }

        //rotating based on mouse position
        //rb.rotation =  Quaternion.Euler(-Input.mousePosition.y , Input.mousePosition.x, 0);

        //move based on axis input
        transform.Translate(
            Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed,
            0,
            Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed,
            Space.Self
            );


        //on click do a swing
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("swing");
            
        }
    }

    // Knock back method for if hit
    public void knockback(Quaternion rotation, float knockback)
    {
        Debug.Log("knockback");
        //send them back from where enemy is facing
        rb.AddForce(rotation * Vector3.back * -knockback);
    }

    public void OnTriggerStay(Collider other)
    {
        if (gameManager.CurrentPlayingState != GameManager.PlayingState.Normal)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Interacting!");

            if (other.CompareTag("interactArea"))
            {
                //Debug.Log("in interact area");
                // TODO: Add special logic for each different kind of interactArea
                // TODO: Probably just use different tags for each one!
            }
            else if (other.CompareTag("enemy"))
            {
                persistentPlayer.TryEquipEnemyAsHost(other.gameObject);
            }
        }
    }
}
