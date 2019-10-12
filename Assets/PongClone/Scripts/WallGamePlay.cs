using System;
using System.Collections;
using UnityEngine;

namespace PongClone
{
    public class WallGamePlay : Gameplay
    {
        #region Setting
        public int topScore;
        #endregion

        [SerializeField] private WallGameplayUI _ui = null;

        public override void ManualStart()
        {
            Debug.LogFormat("WallGamePlay: {0}", GetInstanceID());
            if (rightHandedness)
            {
                Initialize(leftWall.gameObject, rightPlayer, typeof(WallPlayer));
                rightWall.side = me;
                rightWall.onHitBall = FailToReturn;
            }
            else
            {
                Initialize(leftPlayer, rightWall.gameObject, typeof(WallPlayer));
                leftWall.side = me;
                leftWall.onHitBall = FailToReturn;
            }
            me.preHitObject += AddScore;

            _ui.SetHandedness(rightHandedness);

            me.StartPlay();
            opponent.StartPlay();
            StartCoroutine(IncreaseBallSpeed());
        }

        protected override void Initialize(GameObject leftPlayer, GameObject rightPlayer, Type opponentType)
        {
            base.Initialize(leftPlayer, rightPlayer, opponentType);
            me.gameObject.SetActive(true);
            opponent.GetComponent<SpriteRenderer>().enabled = true;
            opponent.GetComponent<Rigidbody2D>().isKinematic = true;
            opponent.GetComponent<Collider2D>().isTrigger = false;
        }

        private IEnumerator IncreaseBallSpeed()
        {
            WaitForSeconds wait = new WaitForSeconds(5);
            while (ball.speed < Gameplay.MAX_SPEED)
            {
                yield return wait;
                ball.speed++;
            }
        }

        private void AddScore(GameObject player)
        {
            me.Point++;
            _ui.UpdatePlayerPoint(me.Point, rightHandedness);
            if (me.Point > globalData.LastSessionPoint)
            {
                globalData.LastSessionPoint = me.Point;
            }
        }

        private void FailToReturn(Wall wall)
        {
            Fail(wall.side);
            _ui.EndMatch();
        }
    }
}