using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Unlike PlayerController, this stores player info that is persistent,
/// even after changing between multiple hosts.
/// </summary>
public class PersistentPlayer : MonoBehaviour
{
    /// <summary>
    /// The player's final health reserves when outside of a host.
    /// </summary>
    public float MaxMaskHealth = 10f;
    private float CurrentMaskHealth;

    //int maskID = 1;
    public GameObject equippedMask;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        CurrentMaskHealth = MaxMaskHealth;
    }

    public void CountDownMaskLife()
    {
        if (0 >= CurrentMaskHealth)
        {
            Debug.Log("Lost final health; game over!");
            gameManager.ChangePlayingState(GameManager.PlayingState.GameOver);
        }
        else
        {
            CurrentMaskHealth -= Time.deltaTime;
        }
    }


    // Update is called once per frame.
    void Update()
    {
        // TODO: Count down MaskHealth when no masks are equipped.

        if (equippedMask != null)
        {
            // A mask is equipped.
            //equippedMask.GetComponent<Host>().IsEquipped = true;
        }
        else
        {
            // No mask is equipped.
        }
    }

    public void equipMask(GameObject other)
    {
        other.transform.position = new Vector3(0,0,0);
        other.transform.tag = "area";
        equippedMask.transform.parent.position = new Vector3(0,0,0);
        if(other.transform.parent.gameObject.tag == "enemy"){
            other.transform.parent.gameObject.GetComponent<EnemyCatController>().MaskControl();
        }
        equippedMask.transform.parent.tag = "interactArea";
        //equippedMask.GetComponent<Host>().IsEquipped = false;
        equippedMask.transform.parent.SetParent(GameObject.Find("droppedMasks").transform);
        Debug.Log(other);
        equippedMask = other.transform.GetChild(0).gameObject;
        Debug.Log(equippedMask);
        //equippedMask.GetComponent<Host>().IsEquipped = true;
        equippedMask.transform.parent.SetParent(transform);
        Debug.Log(equippedMask.transform.parent.position + " name: " + equippedMask.transform.parent.name);
        equippedMask.transform.parent.position = new Vector3(0,0,0);
        Debug.Log(equippedMask.transform.parent.position + " name: " + equippedMask.transform.parent.name);



    }
}
