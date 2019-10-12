using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongClone
{
    public class Wall : MonoBehaviour
    {
        public BasePlayer side;
        public Action<Wall> onHitBall;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.attachedRigidbody && collider.attachedRigidbody.CompareTag(Definition.BALL))
            {
                onHitBall?.Invoke(this);
            }
        }
    }
}