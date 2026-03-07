using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(HealthComponent))]
    public class DrainHealthOverTimeComponent : MonoBehaviour
    {
        private HealthComponent healthState;

        private void Start()
        {
            healthState = GetComponent<HealthComponent>();
        }

        private void Update()
        {
            if (healthState.IsAlive)
            {
                //Debug.Log(transform.name + "is losing life: " + healthState.CurrentHealth);
                healthState.ModifyHealth(-Time.deltaTime);
            }
        }
    }
}