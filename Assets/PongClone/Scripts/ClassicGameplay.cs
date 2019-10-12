using System;
using UnityEngine;

namespace PongClone
{
    public class ClassicGameplay : Gameplay
    {
        public int endingPoint = 3;

        [SerializeField] private GameplayUI _ui = null;

        public override void ManualStart()
        {
            Initialize(leftPlayer, rightPlayer, typeof(WatchDogPlayer));
            _ui.PointsToWin = endingPoint = globalData.PointsToWin;
        }

        protected override void Initialize(GameObject leftPlayer, GameObject rightPlayer, Type opponentType)
        {
            base.Initialize(leftPlayer, rightPlayer, opponentType);
            leftWall.side = leftPlayer.GetComponent<BasePlayer>();
            rightWall.side = rightPlayer.GetComponent<BasePlayer>();
            leftWall.onHitBall = rightWall.onHitBall = UpdatePlayerPoint;

            if (UnityEngine.Random.Range(0, 100) > 49)
            {
                ToggleTurn();
            }

            me.StartPlay();
            opponent.StartPlay();
        }

        private void ToggleTurn()
        {
            me.myTurn = !me.myTurn;
            opponent.myTurn = !opponent.myTurn;
        }

        public void UpdatePlayerPoint(Wall wall)
        {
            ((IPause)me).Pause();
            ((IPause)opponent).Pause();
            if (wall.side == me)
            {
                opponent.Point++;
                _ui.UpdatePlayerPoint(opponent, me, RestartRound, EndMatch);
            }
            else if (wall.side == opponent)
            {
                me.Point++;
                _ui.UpdatePlayerPoint(me, opponent, RestartRound, EndMatch);
            }
        }

        private void RestartRound()
        {
            ToggleTurn();
            me.StartPlay();
            opponent.StartPlay();
            Debug.Log("RestartRound");
        }

        private void EndMatch(BasePlayer loser)
        {
            Fail(loser);
            _ui.EndMatch();
            Debug.Log("EndMatch");
        }
    }
}