using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(HealthComponent))]
    public class OnPlayerFinalHealthChangeComponent : MonoBehaviour
    {
        private HealthComponent playerFinalHealth;

        private void Start()
        {
            playerFinalHealth = GetComponent<HealthComponent>();

            playerFinalHealth.OnDeath += OnPlayerDeath;
            playerFinalHealth.OnHealthChanged += OnPlayerHealthChanged;

            // Initialize health UI.
            OnPlayerHealthChanged(
                playerFinalHealth.CurrentHealth,
                playerFinalHealth.maxHealth
            );
        }


        private void OnPlayerHealthChanged(float newHealth, float amountChanged)
        {
            GameManager.Instance.UpdatePlayerFinalHealthUI(
                playerFinalHealth.CurrentHealth,
                playerFinalHealth.maxHealth
            );
        }

        private void OnPlayerDeath()
        {
            // TODO: Sound effect, animation, etc.

            Debug.Log("Lost final health; game over!");

            GameManager.Instance.ChangePlayingState(
                GameManager.PlayingState.GameOver
            );
            
            GameManager.Instance.UpdatePlayerFinalHealthUI(
                0,
                playerFinalHealth.CurrentHealth
            );
        }
    }
}