using System.Collections;
using UnityEngine;

namespace PongClone
{
    public class WallPlayer : BasePlayer, IPause
    {
        protected override void Start()
        {
            preHitObject += (o) => RandomDeltaY();
        }

        private void RandomDeltaY()
        {
            if (Ball.TooSteep())
            {
                int d = Ball.Direction;
                if (d > 0)
                {
                    deltaY = Random.Range(-d * speed * Time.fixedDeltaTime, -d * 0.25f * speed * Time.fixedDeltaTime);
                }
                else
                {
                    deltaY = Random.Range(-d * 0.25f * speed * Time.fixedDeltaTime, -d * speed * Time.fixedDeltaTime);
                }
            }
            else
            {
                deltaY = Random.Range(-speed * Time.fixedDeltaTime, speed * Time.fixedDeltaTime);
            }
            deltaY *= 2;
        }

        protected override IEnumerator BeforeLaunchBall()
        {
            yield return null;
        }

        #region IPause
        void IPause.Pause()
        {
            runtimeSpeed = 0;
        }

        void IPause.Play()
        {
            runtimeSpeed = speed;
        }
        #endregion
    }
}