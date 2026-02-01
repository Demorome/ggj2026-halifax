using UnityEngine;

/*
public class groundCheck : MonoBehaviour
{
    public GameObject playerBody;
    public Collider Collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    //if you exit as in leave the collision it checks if the object you left collision with was environment and if so you cant jump again
    public void OnTriggerExit(Collider collObj)
    {
        if (collObj.tag == "environment")
        {
            //Debug.Log("exit: " + collObj);
            playerBody.GetComponent<PlayerController>().grounded = false;
        }
    }
    //collObj = colliding object
    public void OnTriggerEnter(Collider collObj)
    {
        if (collObj.tag == "environment")
        {
            //Debug.Log("Enter: " + collObj);
            //Debug.Log(collObj.tag);
            playerBody.GetComponent<PlayerController>().grounded = true;
        }
    }

    
}

*/