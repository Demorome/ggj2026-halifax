using UnityEngine;

/// <summary>
/// The game is setup such that each EnemyEntity has a Host counterpart,
/// for when the player controls them.
///
/// When the player possesses an EnemyEntity, that entity gets disabled.
/// It is replaced with a look-alike Host entity, controlled by the player.
///
/// When the player leaves a Host, the Host gets disabled
/// and replaced with its corresponding EnemyEntity.
/// </summary>
public abstract class Host : MonoBehaviour
{
    public abstract float maxHostHealthTimer { get; set; }
    protected abstract float CurrentHostHealthTimer { get; set; }

    public abstract float mass { get; set; }
    public abstract PlayerController playerController { get; set; }

    protected EnemyEntity ReplacedEntity { get; set; }

    public bool IsEquipped => ReplacedEntity != null;

    protected void CountDownHostLife()
    {
        if (CurrentHostHealthTimer > 0)
        {
            //Debug.Log(transform.name + "is losing life: " + CurrentHostHealthTimer);
            CurrentHostHealthTimer -= Time.deltaTime;
            GameManager.Instance.UpdateHostHealthUI(CurrentHostHealthTimer, maxHostHealthTimer);
        }
        else
        {
            Debug.Log("Host died!");

            // TODO: Sound effect, animation, etc.

            GameManager.Instance.UpdateHostHealthUI(0f, maxHostHealthTimer);

            // Re-enable the disabled entity.
            if (ReplacedEntity != null)
            {
                ReplacedEntity.gameObject.SetActive(true);
                ReplacedEntity.transform.position = transform.position;
                ReplacedEntity = null;
                // TODO: Make it inactive (dead)
                // TODO: Change its final pose (re-use unconscious pose).

                // Host is now a corpse, so we may as well destroy it,
                // since we'll never be able to possess it again.
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("There should've been a Replaced Entity here!");
            }
        }
    }

}