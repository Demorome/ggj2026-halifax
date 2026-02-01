using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

/// <summary>
/// NOTE: The player may switch from host to host, which may use different PlayerControllers!
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody rb;

    public enum HostType { None_RegularMask, Mouse, Cat, Human, Deer };
    public HostType hostType;

    public Collider collider;

    private GameManager gameManager;
    private PersistentPlayer persistentPlayer;

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

        if (other.CompareTag("interactArea"))
        {
            //Debug.Log("in interact area");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("interacting");
                maskHolder.GetComponent<PersistentPlayer>().equipMask(other.gameObject);

            }
        }
    }
}
