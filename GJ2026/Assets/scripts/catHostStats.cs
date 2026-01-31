using Unity.VisualScripting;
using UnityEngine;

public class hostStats : host
{
    public float timer = 10f;
    private float maxTimer = 20f;
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
            timer = timer-Time.deltaTime;
            playerController.updateHealth(timer);
           //Debug.Log(timer);
        }
    }
}
