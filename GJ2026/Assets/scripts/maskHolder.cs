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
        equippedMask.transform.parent.tag = "interactArea";
        equippedMask.GetComponent<host>().equipped = false;
        equippedMask.transform.parent.SetParent(GameObject.Find("droppedMasks").transform);
        Debug.Log(other);
        equippedMask = other.transform.GetChild(0).gameObject;
        Debug.Log(equippedMask);
        equippedMask.GetComponent<host>().equipped = true;
        equippedMask.transform.parent.SetParent(transform, false);


    }
}
