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

    /// <summary>
    /// Will be disabled when the player enters a host,
    /// and re-enabled when leaving the host.
    /// </summary>
    public PlayerController controllerWhenInRegularForm;

    public Host equippedHost;
    public bool IsHostEquipped => equippedHost != null;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        CurrentMaskHealth = MaxMaskHealth;
    }

    // Update is called once per frame.
    void Update()
    {
        if (gameManager.CurrentPlayingState != GameManager.PlayingState.Normal)
        {
            return;
        }

        if (equippedHost != null)
        {
            // A mask is equipped.
            //
        }
        else
        {
            // No mask is equipped.
            CountDownMaskLife();
        }
    }

    public void equipMask(GameObject other)
    {
        other.transform.position = new Vector3(0,0,0);
        other.transform.tag = "area";
        equippedHost.transform.parent.position = new Vector3(0,0,0);
        if(other.transform.parent.gameObject.tag == "enemy"){
            other.transform.parent.gameObject.GetComponent<EnemyCatController>().MaskControl();
        }
        equippedHost.transform.parent.tag = "interactArea";
        //equippedMask.GetComponent<Host>().IsEquipped = false;
        equippedHost.transform.parent.SetParent(GameObject.Find("droppedMasks").transform);
        Debug.Log(other);
        equippedHost = other.transform.GetChild(0).gameObject;
        Debug.Log(equippedHost);
        //equippedMask.GetComponent<Host>().IsEquipped = true;
        equippedHost.transform.parent.SetParent(transform);
        Debug.Log(equippedHost.transform.parent.position + " name: " + equippedHost.transform.parent.name);
        equippedHost.transform.parent.position = new Vector3(0,0,0);
        Debug.Log(equippedHost.transform.parent.position + " name: " + equippedHost.transform.parent.name);
    }

    private void CountDownMaskLife()
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
}
