using UnityEngine;
using TMPro;


public class playerController : MonoBehaviour
{
 public GameObject groundCheck;
    public bool grounded = true;
    public float moveSpeed = 10f;
    public float jumpHeight = 100f;
    public Rigidbody rb;

    private Vector3 jump =  Vector3.zero;
    public GameObject maskHolder;
    public Collider collider;
    public float health = 42f;
    public GameObject healthText;
    public float finalHealth = 20f;
    public GameObject sceneManager;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateHealth(42f);
    }


    // Update is called once per frame
    void Update()
    {
        //check if the groundchecker is touching or not
        if (grounded == true)
        {

            //down means it can only be pressed not held down note to self think about jetpack for holding down
            if (Input.GetKeyDown("space"))
            {
                Debug.Log("jump");
                //adding force up
                jump.y = jumpHeight * Time.deltaTime * 1000;
                rb.AddForce(transform.up * jumpHeight);

            }
        }
    }
    void FixedUpdate()
    {

        //rotating based on mouse position
        //rb.rotation =  Quaternion.Euler(-Input.mousePosition.y , Input.mousePosition.x, 0);

        //move based on axis input
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed, Space.Self);


        //on click do a swing
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("swing");
            
        }
        

    }
    //knock back method for if hit
    public void knockback(Quaternion rotation, float knockback)
    {
        Debug.Log("knockback");
        //send them back from where enemy is facing
        rb.AddForce(rotation * Vector3.back * -knockback);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("interactArea"))
        {
            Debug.Log("in interact area");
            if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("interacting");
            maskHolder.GetComponent<maskHolder>().equipMask(other.gameObject);
            
        }
        }
    }
        public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("interactArea"))
        {
            //Debug.Log("in interact area");
            if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("interacting");
            maskHolder.GetComponent<maskHolder>().equipMask(other.gameObject);
            
        }
        }
    }
    public void updateHealth(float newHealth)
    {
        healthText.GetComponent<TextMeshProUGUI>().text = "Health: " + newHealth + "\n Final Health: " + finalHealth;

    }
    public void loseFinalHealth()
    {
        if(0 >= finalHealth)
        {
            sceneManager.GetComponent<sceneManager>().gameOver();
        }
        else{
            finalHealth = finalHealth - Time.deltaTime;
            updateHealth(0f);
        }
    }
}
