using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(HealthComponent))]
    public class OnHostLifeChangeComponent : MonoBehaviour
    {
        private HealthComponent hostHealth;
        
        private void Start()
        {
            hostHealth = GetComponent<HealthComponent>();

            hostHealth.OnDeath += HostHealthOnDeath;
            hostHealth.OnHealthChanged += HostHealthOnHealthChanged;
        }

        private void HostHealthOnHealthChanged(float newHealth, float amountChanged)
        {
            GameManager.Instance.UpdatePlayerHostHealthUI(
                hostHealth.CurrentHealth,
                hostHealth.maxHealth
            );
        }

        private void HostHealthOnDeath()
        {
            Debug.Log("Host died!");

            // TODO: Sound effect, animation, etc.

            // TODO: Port this code elsewhere, in an event Action listener!
            /*
            GameManager.Instance.UpdatePlayerHostHealthUI(0f, hostHealth.maxHealth);

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
            }*/
        }
    }
}