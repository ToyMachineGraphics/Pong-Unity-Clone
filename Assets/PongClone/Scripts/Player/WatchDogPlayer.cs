using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongClone
{
    public class WatchDogPlayer : BasePlayer, IPause
    {
        protected override IEnumerator BeforeLaunchBall()
        {
            if (myTurn)
            {
                HoldBall();
                float time = 0;
                WaitForFixedUpdate wait = new WaitForFixedUpdate();
                while (time < 2)
                {
                    float duration = 0.5f;
                    float subTime = 0;
                    direction = Mathf.Sign(Random.Range(-1f, 1f));
                    while (time < 2 && subTime < duration)
                    {
                        time += Time.deltaTime;
                        subTime += Time.fixedDeltaTime;
                        FixedUpdate();
                        yield return wait;
                    }
                    yield return null;
                }
            }
        }

        private void Update()
        {
            direction = Mathf.Clamp(Ball.transform.position.y - transform.position.y, -1, 1);
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