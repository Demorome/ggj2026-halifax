using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(HealthComponent))]
    public class DrainHealthOverTimeComponent : MonoBehaviour
    {
        private HealthComponent myHealthState;
        private HealthComponent receiverHealthState;

        // The receiver of the drained health might receive more or less.
        private const float DrainHealthReceiverMult = 0.5f;

        public void SetRecipient(GameObject recipient)
        {
            receiverHealthState = recipient.GetComponent<HealthComponent>();
        }

        private void Start()
        {
            myHealthState = GetComponent<HealthComponent>();
        }

        private void Update()
        {
            if (!myHealthState.IsAlive) return;

            //Debug.Log(transform.name + "is losing life: " + healthState.CurrentHealth);
            var drainedLife = Time.deltaTime;
            myHealthState.ModifyHealth(-drainedLife);

            if (receiverHealthState && receiverHealthState.IsAlive)
            {
                receiverHealthState.ModifyHealth(drainedLife * DrainHealthReceiverMult);
            }
        }
    }
}