using UnityEngine;

public class maskHolder : MonoBehaviour
{
    int mask = 1;
    public GameObject equippedMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(equippedMask != null)
        {
            equippedMask.GetComponent<host>().equipped = true;
        }
    }

    public void equipMask(GameObject other)
    {
        Debug.Log(other);
    }
}
