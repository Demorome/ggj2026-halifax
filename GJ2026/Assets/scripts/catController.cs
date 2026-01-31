using UnityEngine;

public class catController : MonoBehaviour
{
    public GameObject detection;
    public Rigidbody rb;
    public GameObject player;
    public Rigidbody playerRB;
    public GameObject enemySelf;
    public Collider collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detection.GetComponent<enemyDetectionEventHandler>().seek == true)
        {
            Debug.Log("seeking");
            //get player rigidbody
            //move
            //rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, player., 360))
            //find the rotation through looking over and rotating forward to payer poisiton
            float max = 360.0f;
            rb.rotation = Quaternion.LookRotation(Vector3.RotateTowards(Vector3.forward, new Vector3(playerRB.position.x - rb.position.x, 0f, playerRB.position.z - rb.position.z), max, 0.0f));
            rb.MovePosition(rb.position + 1f * (rb.rotation * Vector3.forward) * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.GetComponent<playerController>().finalHealth--;
        }
    }
}
