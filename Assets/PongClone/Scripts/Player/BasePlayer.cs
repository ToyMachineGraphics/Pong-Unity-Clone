using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PongClone
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BasePlayer : MonoBehaviour
    {
        public int id;
        public float speed = 1;
        protected float runtimeSpeed;

        public Vector3 OriginalPosition { get; private set; }

        public float direction;
        public Direction facing;

        public BaseBall Ball { protected get; set; }
        public Action<GameObject> preHitObject;

        public bool myTurn;

        [SerializeField] private int _point;
        public int Point
        {
            get { return _point; }
            set
            {
                _point = value;
                onSetPoint?.Invoke(value);
            }
        }
        public Action<int> onSetPoint;

        private float _oldPositionY;
        protected float deltaY;

        private Rigidbody2D _rigidbody2D;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Vector2 _colliderSize;
        private RaycastHit2D[] _raycastHits = new RaycastHit2D[1];
        [SerializeField] private float _bottomY;
        [SerializeField] private float _topY;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Color _originalColor;

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
        }

        protected virtual void Start()
        {
            Debug.Log("BasePlayer Start");
            enabled = false;
            Initialize();
        }

        private void Initialize()
        {
            OriginalPosition = transform.position;
            _oldPositionY = transform.position.y;

            _colliderSize.x = _collider.size.x * transform.localScale.x;
            _colliderSize.y = _collider.size.y * transform.localScale.y;

            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask(Definition.WALL);
            filter.useDepth = true;
            filter.minDepth = filter.maxDepth = 0;
            int count = _collider.Raycast(Vector2.down, filter, _raycastHits);
            _bottomY = _raycastHits[0].point.y;
            count = _collider.Raycast(Vector2.up, filter, _raycastHits);
            _topY = _raycastHits[0].point.y;

            Reset();
        }

        public void Reset()
        {
            runtimeSpeed = speed;
        }

        public void StartPlay()
        {
            StopAllCoroutines();
            StartCoroutine(StartPlayTask());
        }

        private IEnumerator StartPlayTask()
        {
            Reset();
            yield return BeforeLaunchBall();
            enabled = true;

            if (myTurn)
            {
                ServeBall();
            }
        }

        protected abstract IEnumerator BeforeLaunchBall();

        protected virtual void FixedUpdate()
        {
            deltaY = transform.position.y - _oldPositionY;
            _oldPositionY = transform.position.y;
            Vector2 d = Vector2.up * direction * runtimeSpeed * Time.fixedDeltaTime;
            MovePosition(d);
        }

        protected void MovePosition(Vector2 deltaMovement)
        {
            Vector2 destination = _rigidbody2D.position + deltaMovement;
            if (deltaMovement.y > 0 && destination.y > _topY - _colliderSize.y / 2)
            {
                _rigidbody2D.MovePosition(new Vector2(destination.x, _topY - _colliderSize.y / 2));
            }
            else if (deltaMovement.y < 0 && destination.y < _bottomY + _colliderSize.y / 2)
            {
                _rigidbody2D.MovePosition(new Vector2(destination.x, _bottomY + _colliderSize.y / 2));
            }
            else
            {
                _rigidbody2D.MovePosition(destination);
            }
        }

        public void HoldBall()
        {
            Ball.OnHeld();
            Transform ball = Ball.transform;
            ball.SetParent(transform);
            ball.position = transform.position;
            float distance = 0.5f;
            if (facing == Direction.Left)
            {
                ball.position = transform.position + Vector3.left * distance;
            }
            else
            {
                ball.position = transform.position + Vector3.right * distance;
            }
        }

        public void ServeBall()
        {
            Ball.transform.SetParent(null);
            if (facing == Direction.Left)
            {
                Ball.SetVelocity(Vector2.left);
            }
            else
            {
                Ball.SetVelocity(Vector2.right);
            }
        }

        public void AddImpulseToBall(BaseBall ball)
        {
            deltaY = (ball.transform.position - transform.position).y * 2;
            Vector2 deltaV = Vector2.up * deltaY * speed * 1.5f;
            ball.AddVelocity(deltaV);
        }

        public void Blink()
        {
            if (_spriteRenderer.color == _originalColor)
            {
                _spriteRenderer.color = _originalColor * Color.grey;
            }
            else
            {
                _spriteRenderer.color = _originalColor * Color.white;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Definition.BALL))
            {
                preHitObject?.Invoke(collision.gameObject);
                BaseBall ball = collision.gameObject.GetComponent<BaseBall>();
                AddImpulseToBall(ball);
            }
        }
    }
}