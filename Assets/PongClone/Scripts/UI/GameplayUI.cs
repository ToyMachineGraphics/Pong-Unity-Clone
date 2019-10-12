using System;
using System.Collections;
using UnityEngine;

namespace PongClone
{
    public class GameplayUI : BaseGameplayUI
    {
        private const float MAX_POINT_TO_WIN = 7;
        private int _pointsToWin;
        public int PointsToWin
        {
            get { return _pointsToWin; }
            set
            {
                _pointsToWin = value;
                _pointIndexInterval = MAX_POINT_TO_WIN / value;
            }
        }
        private float _pointIndexInterval;

        public Sprite[] bars;

        public void UpdatePlayerPoint(BasePlayer winner, BasePlayer loser, Action onRoundComplete, Action<BasePlayer> onMatchComplete)
        {
            _points[winner.id].text = winner.Point.ToString();

            SpriteRenderer sr = loser.GetComponent<SpriteRenderer>();
            int index = Mathf.RoundToInt(winner.Point * _pointIndexInterval);
            Debug.LogFormat("index: {0}", index);
            if (index > 6)
            {
                sr.sprite = bars[(int)MAX_POINT_TO_WIN - 1];
                StartCoroutine(WinLoseAnim(winner, loser, onMatchComplete));
            }
            else
            {
                StartCoroutine(PointShine(winner.id, onRoundComplete));
                sr.sprite = bars[index];
            }
        }

        private IEnumerator WinLoseAnim(BasePlayer winner, BasePlayer loser, Action<BasePlayer> onComplete)
        {
            yield return StartCoroutine(PointShine(winner.id, null));
            onComplete(loser);
            Debug.Log("WinLoseAnim");
        }
    }
}