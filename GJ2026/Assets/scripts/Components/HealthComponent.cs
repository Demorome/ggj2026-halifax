using System;
using UnityEngine;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        public float CurrentHealth { get; private set; } = 10f;
        public float maxHealth = 10f;

        /// <summary>
        /// Pass a negative value to remove health.
        /// </summary>
        public void ModifyHealth(float amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > maxHealth)
            {
                CurrentHealth = maxHealth;
            }
            OnHealthChanged?.Invoke(CurrentHealth, amount);

            if (CurrentHealth <= 0f && !hasDied)
            {
                hasDied = true;
                OnDeath?.Invoke();
            }
        }

        /// <summary>
        /// Passes new currentHealth, and amount modified.
        /// </summary>
        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        public bool IsDead => hasDied;
        public bool IsAlive => !IsDead;

        private bool hasDied = false;

        private void OnValidate()
        {
            // Ensure maxHealth is at least 1
            if (maxHealth < 1)
                maxHealth = 1;

            // Clamp currentHealth between 0 and maxHealth
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        }
    }
}