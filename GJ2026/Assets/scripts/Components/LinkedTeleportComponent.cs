using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider))]
    public class LinkedTeleportComponent : MonoBehaviour
    {
        /// <summary>
        /// WARNING: There will be 2 sources of truth!
        /// Make sure to properly link 2 teleport objects together!
        /// </summary>
        public GameObject teleportToObject;

        public void Teleport(GameObject toTeleport)
        {
            toTeleport.transform.position = teleportToObject.transform.position;
        }
    }
}