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

    public (GameObject, GameObject)? maybeEquippedHostEnemyPair = null;
    public bool IsHostEquipped => maybeEquippedHostEnemyPair != null;

    private GameManager gameManager;
    public GameObject playerHolder;

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
    /// May not succeed if the player already has a host equipped. <br/>
    ///
    /// WARNING: the enemy needs to be part of a "enemyAndHostVersionHolder" object,
    /// and the Host (player-controlled version) needs to be
    /// a pre-Disabled child of the enemy.<br/>
    ///
    /// The Host object will be automatically moved
    /// under "playerHolder" -> "player" -> "hostHolder".
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

        // FIXME: Check the enemy's health / life status!
        // TODO: It might be dead; can't equip it then!!

        // FIXME: simplify!
        //enemyToEquip.transform.parent.gameObject.GetComponent<EnemyCatController>().MaskControl();

        // Host version will always be a pre-disabled child of the enemy object.
        var hostVersion = Transform.FindFirstObjectByType<GameObject>();
        Debug.Log(hostVersion.name);
        maybeEquippedHostEnemyPair = new(hostVersion, enemyToEquip);

        if (!hostVersion.activeInHierarchy)
        {
            Debug.LogError("Host version should have been inactive/disabled!");
            return;
        }
        if (enemyToEquip.activeInHierarchy)
        {
            Debug.LogError("Enemy should have been active/enabled!");
            return;
        }

        // enable the host version
        hostVersion.SetActive(true);
        hostVersion.transform.SetParent(playerHolder.transform);
        hostVersion.transform.position = enemyToEquip.transform.position;

        // disable the enemy version
        enemyToEquip.SetActive(false);
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

        var (host, enemy) = maybeEquippedHostEnemyPair.Value;

        // Re-enable enemy
        enemy.SetActive(true);
        // FIXME: Set the enemy unconscious!!
        // TODO: Change its transform parent back to enemy holder!


        // Disable host (player version)
        host.SetActive(false);
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
