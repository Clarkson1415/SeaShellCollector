﻿using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider2D))]
    /// <summary>
    /// Represents things that can be picked up on the beach by players or critters.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        public PickupType PickupType;
    }
}
