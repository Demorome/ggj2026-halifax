using Unity.VisualScripting;
using UnityEngine;

public class CatHostStats : host
{
    public float timer = 10f;
    private float maxTimer = 20f;
    public float mass = 1f;
    public playerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = maxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(equipped == true)
        {
            if(timer > 0){
                //Debug.Log(transform.name);
                timer = timer-Time.deltaTime;
                playerController.updateHealth(timer);
                //Debug.Log(timer);
            }
            else
            {
                playerController.loseFinalHealth();
            }
        }
    }
}
