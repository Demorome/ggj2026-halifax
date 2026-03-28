using UnityEngine;

namespace Components
{
    /// <summary>
    /// Determines your strength when trying to push objects around. <br/>
    /// If you don't meet a certain threshold,
    /// some objects will be impossible to push in your current form. <br/>
    /// <br/>
    /// Also serves as a rough indication of your size,
    /// which the camera will use to determine the zoom level.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class MassComponent : MonoBehaviour
    {
        [Range (0, 100)]
        public int mass;

        public float MassPercent => mass / 100f;

        // TODO: Implement knocking objects back based on mass!
    }
}