using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(CanBeInteractedComponent))]
    public class CheeseCollectableComponent : MonoBehaviour
    {
        public void Collect()
        {
            GameManager.Instance.SendIncrementObjectiveProgressSignal(ObjectiveType.CollectCheese);
            Destroy(gameObject);
        }
    }
}