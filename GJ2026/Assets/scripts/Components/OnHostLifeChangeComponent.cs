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

            hostHealth.OnDeath += OnDeath;
            hostHealth.OnHealthChanged += OnHealthChanged;

            // Initialize the UI.
            OnHealthChanged(hostHealth.CurrentHealth, hostHealth.maxHealth);
        }

        private void OnHealthChanged(float newHealth, float amountChanged)
        {
            GameManager.Instance.UpdatePlayerHostHealthUI(
                hostHealth.CurrentHealth,
                hostHealth.maxHealth
            );
        }

        private void OnDeath()
        {
            Debug.Log("Host died!");

            // TODO: Sound effect, animation, etc.
            GameManager.Instance.UpdatePlayerHostHealthUI(0f, hostHealth.maxHealth);
        }

        private void OnDestroy()
        {
            // This component will be destroyed when the host is left and/or dies,
            // so update host health UI to reflect that.
            GameManager.Instance.UpdatePlayerHostHealthUI(0f, hostHealth.maxHealth);
        }
    }
}