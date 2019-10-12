using System.Collections;
using UnityEngine;

namespace PongClone
{
    public class ManualPlayer : BasePlayer, IPause
    {
        private Camera _camera;
        public int moveThreshold;
#if !UNITY_EDITOR && UNITY_ANDROID
        private int _fingerId = -1;
#endif

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
        }

        protected override void Start()
        {
            base.Start();
            moveThreshold = Screen.height / 32;
        }

        protected override IEnumerator BeforeLaunchBall()
        {
            if (myTurn)
            {
                HoldBall();
            }
            yield return new WaitForSeconds(1);
        }

        private void Update()
        {
            direction = 0;
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(0))
            {
                Vector2 diff = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
                if (Mathf.Abs(diff.y) > moveThreshold)
                {
                    direction = Mathf.Sign(diff.y);
                }
            }
#elif UNITY_ANDROID
            Vector2 diff = Vector2.zero;
            for (int i = 0; i < Input.touchCount;)
            {
                Touch t = Input.GetTouch(i);
                if (_fingerId == -1)
                {
                    if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                    {
                        _fingerId = t.fingerId;
                    }
                }
                else if (_fingerId == t.fingerId)
                {
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        _fingerId = -1;
                    }
                    Vector2 v = _camera.WorldToScreenPoint(transform.position);
                    diff = t.position - v;
                }
                break;
            }
            if (Mathf.Abs(diff.y) > moveThreshold) {
                direction = Mathf.Sign(diff.y);
            }
#endif
        }

#region IPause
        void IPause.Pause()
        {
            runtimeSpeed = 0;
        }

        void IPause.Play()
        {
            Reset();
        }
#endregion
    }
}