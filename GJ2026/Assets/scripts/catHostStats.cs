using Unity.VisualScripting;
using UnityEngine;

public class CatHostStats : host
{
    public float maxTimer;
    private float CurrentTimer;

    public float mass = 1f;
    public playerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTimer = maxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(equipped == true)
        {
            if(CurrentTimer > 0){
                //Debug.Log(transform.name);
                CurrentTimer -= Time.deltaTime;
                playerController.updateHealth(CurrentTimer);
                //Debug.Log(timer);
            }
            else
            {
                playerController.loseFinalHealth();
            }
        }
    }
}
