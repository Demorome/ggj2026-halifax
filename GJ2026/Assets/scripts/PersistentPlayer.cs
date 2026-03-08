using Components.Actors;
using UnityEngine;

/// <summary>
/// This is always active, even after changing forms/hosts. <br/>
///
/// This object's parent changes when the player changes host/form. <br/>
/// If player in default form, then parent is a PlayerDefaultForm. <br/>
/// Otherwise, parent will inherit from ActorForm -> EnemyActorForm. <br/>
///
/// Handles camera placement, to follow the player. <br/>
/// TODO: May handle special transition logic?
/// </summary>
public class PersistentPlayer : MonoBehaviour
{
    /// <summary>
    /// This object will be disabled when the player enters a host. <br/>
    /// Will be re-enabled when leaving the host,
    /// unless switching immediately to another host.
    /// </summary>
    public PlayerDefaultFormComponent playerDefaultFormComponent;

    public bool IsHostEquipped => !playerDefaultFormComponent.enabled;

    public Camera playerCamera;
    private Vector3 CameraPosOffset;

    void Start()
    {
        if (!IsHostEquipped)
        {
            Debug.Log("Player spawned without a host equipped");
            GameManager.Instance.UpdatePlayerHostHealthUI(0, 100);
        }

        // Save only the Y offset, aka the distance from the
        // camera from a bird's eye view.
        CameraPosOffset = playerCamera.transform.position - transform.position;
        CameraPosOffset.x = 0;
        CameraPosOffset.z = 0;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentPlayingState != GameManager.PlayingState.Normal)
        {
            return;
        }
        /*
         TODO: IMPLEMENT!
        if (IsHostEquipped)
        {
            // Specific PlayerControllers will do host-specific logic,
            // so we just do generic host logic here.
            if (Input.GetKeyDown(KeyCode.X))
            {
                TryUnequipHost();
            }
        }*/
    }

    void LateUpdate()
    {
        playerCamera.transform.position = transform.position + CameraPosOffset;
    }

        /*
    /// <summary>
    /// May not succeed if the player already has a host equipped. <br/>
    ///
    /// WARNING: the Host (player-controlled version) needs to be
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

        if (!enemyToEquip.CompareTag("enemy"))
        {
            Debug.LogError("Can't equip host; target is not an enemy!");
            return;
        }

        // FIXME: Check the enemy's health / life status!
        // TODO: It might be dead; can't equip it then!!

        // FIXME: simplify!
        //enemyToEquip.transform.parent.gameObject.GetComponent<EnemyCatController>().MaskControl();

        // Host version will always be a pre-disabled child of the enemy object.
        var hostVersion = enemyToEquip.transform.GetChild(0).gameObject;
        Debug.Log(hostVersion.name);
        maybeEquippedHostEnemy = new(hostVersion, enemyToEquip);

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
        // FIXME: This will probably break when going back and forth!
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

        var (host, enemy) = maybeEquippedHostEnemy.Value;

        // Re-enable enemy
        enemy.SetActive(true);
        // FIXME: Set the enemy unconscious!!
        enemy.transform.position = host.transform.position;

        // Disable host (player version)
        host.SetActive(false);
        host.transform.SetParent(enemy.transform);
    }*/
}
