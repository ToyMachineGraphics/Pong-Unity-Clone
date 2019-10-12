using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PongClone
{
    public class WallGameplayUI : BaseGameplayUI
    {
        private int _pointIndex;

        public void SetHandedness(bool rightHandedness)
        {
            _pointIndex = rightHandedness ? 1 : 0;
            _points[_pointIndex].gameObject.SetActive(true);
            _points[(_pointIndex + 1) % _points.Length].gameObject.SetActive(false);
        }

        public void UpdatePlayerPoint(int point, bool rightHandedness)
        {
            _points[_pointIndex].text = point.ToString();
            StartCoroutine(PointShine(_pointIndex, null));
        }
    }
}