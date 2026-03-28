using UnityEngine;

namespace Components
{
    public enum ActorType
    {
        PlayerMaskWithoutHost = 0,
        Mouse,
        Cat,
        Dog,
        Human,
    }

    /// <summary>
    /// Describes the type/form of an actor entity. <br/>
    /// The player may take over an enemy, thus taking that form. <br/>
    /// Ex: Cat.
    /// </summary>
    public class ActorTypeComponent : MonoBehaviour
    {
        /// <summary>
        /// WARNING: Should rarely be changed, unless it's the player changing form!
        /// </summary>
        public ActorType actorType;
    }
}