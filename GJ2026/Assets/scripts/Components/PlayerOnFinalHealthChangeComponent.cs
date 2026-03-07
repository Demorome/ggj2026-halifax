using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(HealthComponent))]
    public class PlayerOnFinalHealthChangeComponent : MonoBehaviour
    {
        private HealthComponent playerFinalHealth;

        private void Start()
        {
            playerFinalHealth = GetComponent<HealthComponent>();

            playerFinalHealth.OnDeath += OnPlayerDeath;
            playerFinalHealth.OnHealthChanged += OnPlayerHealthChanged;
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