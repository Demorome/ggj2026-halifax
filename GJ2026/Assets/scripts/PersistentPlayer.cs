using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Unlike PlayerController, this stores player info that is persistent,
/// even after changing between multiple hosts. <br/>
///
/// This is only persistent during a level, and will get reset if the level is reset.
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

    public (Host, EnemyEntity)? maybeEquippedHostEnemyPair = null;
    public bool IsHostEquipped => maybeEquippedHostEnemyPair != null;

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

        if (IsHostEquipped)
        {
            // Specific PlayerControllers will do host-specific logic,
            // so we just do generic host logic here.
            if (Input.GetKeyDown(KeyCode.X))
            {
                TryUnequipHost();
            }
        }
        else
        {
            CountDownMaskLife();
        }
    }

    /// <summary>
    /// May not succeed if the player already has a host equipped.
    /// </summary>
    public void TryEquipEnemyAsHost(GameObject enemyToEquip)
    {
        if (IsHostEquipped)
        {
            Debug.Log("Can't equip host; we already have one equipped. Unequip first!");
            return;
        }

        if (enemyToEquip.tag != "enemy")
        {
            Debug.LogError("Can't equip host; target is not an enemy!");
            return;
        }

        // FIXME: simplify!
        //enemyToEquip.transform.parent.gameObject.GetComponent<EnemyCatController>().MaskControl();

        maybeEquippedHostEnemyPair

        enemyToEquip.transform.position = Vector3.zero;
        //other.transform.tag = "area";
        var enemyParent = enemyToEquip.enemyToEquip
        equippedHost.transform.parent.position = Vector3.zero;


        equippedHost.transform.parent.tag = "interactArea";
        equippedMask.GetComponent<Host>().IsEquipped = false;
        //equippedHost = enemyToEquip.transform.GetChild(0).gameObject as Host;
        equippedMask.GetComponent<Host>().IsEquipped = true;
        equippedHost.transform.parent.SetParent(transform);
        equippedHost.transform.parent.position = Vector3.zero;


        //Debug.Log(other);
        //Debug.Log(equippedHost);
        //Debug.Log(equippedHost.transform.parent.position + " name: " + equippedHost.transform.parent.name);
    }

    /// <summary>
    /// Won't work if no host is equipped.
    /// </summary>
    public void TryUnequipHost()
    {
        if (!IsHostEquipped)
        {
            Debug.Log("Can't unequip host; none is equipped!");
            return;
        }

        // TODO

        equippedHost.
        equippedHost.transform.parent.SetParent(
            GameObject.Find("UnconsciousEnemies").transform
            );
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
