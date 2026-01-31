using Unity.VisualScripting;
using UnityEngine;

public class hostStats : host
{
    public float timer = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(equipped == true)
        {
            timer = timer-Time.deltaTime;
            Debug.Log(timer);
        }
    }
}
