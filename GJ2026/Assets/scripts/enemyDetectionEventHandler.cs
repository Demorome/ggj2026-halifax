using UnityEngine;

public class enemyDetectionEventHandler : MonoBehaviour
{
    public bool seek = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //trigger when player jumps on
    public void OnTriggerEnter(Collider collObj)
    {
        //check object tag to see if player
        if (collObj.tag == "Player")
        {
            seek = true;
            //Debug.Log("Enter: " + collObj);
            //Debug.Log(collObj.tag);

            //note add enemy seek scripts here later
        }
    }
    //check if object is leaving
    public void OnTriggerExit(Collider collObj)
    {
        //check object tag of obj leaving to see if player
        if (collObj.tag == "Player")
        {
            seek = false;
            //Debug.Log("exit: " + collObj);
        }
    }
    
}

