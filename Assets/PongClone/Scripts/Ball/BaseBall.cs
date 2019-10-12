using System;
using UnityEngine;

namespace PongClone
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBall : MonoBehaviour
    {
        public float speed;
        public int Direction
        {
            get
            {
                if (_rigidbody2D.velocity.x == 0)
                {
                    return 0;
                }
                return Mathf.RoundToInt(Mathf.Sign(_rigidbody2D.velocity.y / Mathf.Abs(_rigidbody2D.velocity.x)));
            }
        }

        private Rigidbody2D _rigidbody2D;
        private TrailRenderer _trail;

        public Action<GameObject> onHitCeilingFloor;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _trail = GetComponentInChildren<TrailRenderer>();
        }

        private void FixedUpdate()
        {
            if (_rigidbody2D.velocity.sqrMagnitude != speed * speed)
            {
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * speed;
            }
        }

        public void OnHeld()
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.simulated = false;
            _trail.emitting = false;
        }

        public void SetVelocity(Vector2 direction, float magnitude = -1)
        {
            float finalMagnitude = (magnitude == -1) ? speed : magnitude;
            bool move = finalMagnitude > 0;
            _rigidbody2D.simulated = _trail.emitting = move;
            Vector2 targetVelocity = direction * finalMagnitude;
            Vector2 deltaVelocity = targetVelocity - _rigidbody2D.velocity;
            _rigidbody2D.AddForce(deltaVelocity * _rigidbody2D.mass, ForceMode2D.Impulse);
        }

        public bool TooSteep()
        {
            float slope = _rigidbody2D.velocity.y / Mathf.Abs(_rigidbody2D.velocity.x);
            if (Mathf.Abs(slope) >= Definition.MAX_SLOPE)
            {
                return true;
            }
            return false;
        }

        public void AddVelocity(Vector2 deltaV)
        {
            _rigidbody2D.AddForce(deltaV * _rigidbody2D.mass, ForceMode2D.Impulse);
            if (Mathf.Abs(_rigidbody2D.velocity.y / _rigidbody2D.velocity.x) > Definition.MAX_SLOPE)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Sign(_rigidbody2D.velocity.y) * Mathf.Abs(_rigidbody2D.velocity.x) * Definition.MAX_SLOPE).normalized * speed;
            }
            else
            {
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * speed;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Definition.WALL))
            {
                onHitCeilingFloor(collision.gameObject);
            }
        }
    }
}