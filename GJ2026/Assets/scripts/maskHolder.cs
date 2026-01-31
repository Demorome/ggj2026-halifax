using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

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
        other.transform.position = new Vector3(0,0,0);
        other.transform.tag = "area";
        equippedMask.transform.parent.position = new Vector3(0,0,0);
        if(other.transform.parent.gameObject.tag == "enemy"){
            other.transform.parent.gameObject.GetComponent<catController>().MaskControl();
        }
        equippedMask.transform.parent.tag = "interactArea";
        equippedMask.GetComponent<host>().equipped = false;
        equippedMask.transform.parent.SetParent(GameObject.Find("droppedMasks").transform);
        Debug.Log(other);
        equippedMask = other.transform.GetChild(0).gameObject;
        Debug.Log(equippedMask);
        equippedMask.GetComponent<host>().equipped = true;
        equippedMask.transform.parent.SetParent(transform);
        Debug.Log(equippedMask.transform.parent.position + " name: " + equippedMask.transform.parent.name);
        equippedMask.transform.parent.position = new Vector3(0,0,0);
        Debug.Log(equippedMask.transform.parent.position + " name: " + equippedMask.transform.parent.name);



    }
}
