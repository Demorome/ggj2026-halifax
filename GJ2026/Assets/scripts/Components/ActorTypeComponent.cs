using System.Collections.Generic;
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
        Unknown
    }

    /// <summary>
    /// Describes the type/form of an actor entity. <br/>
    /// The player may take over an enemy, thus taking that form. <br/>
    /// Ex: Cat.
    /// </summary>
    public class ActorTypeComponent : MonoBehaviour
    {
        private static readonly Dictionary<ActorType, Color> ActorUIColors = new()
        {
            { ActorType.PlayerMaskWithoutHost, Color.royalBlue },
            { ActorType.Mouse, Color.yellow },
            { ActorType.Cat, Color.brown },
            { ActorType.Dog, Color.orangeRed },
            { ActorType.Human, Color.crimson },
            { ActorType.Unknown, Color.purple }
        };

        public static Color ColorForActorType(ActorType type)
        {
            return ActorUIColors[type];
        }

        public Color ActorUIColor => ColorForActorType(actorType);

        /// <summary>
        /// WARNING: Should rarely be changed, unless it's the player changing form!
        /// </summary>
        public ActorType actorType;
    }
}