using System;
using UnityEngine;
using UnityEngine.Events;

namespace PongClone
{
    [Serializable]
    public class ColliderEvent2D : UnityEvent<Collider2D> { }
}