using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PongClone
{
    public class BillboardPage : MonoBehaviour
    {
        public GlobalData globalData;
        [SerializeField] RectTransform _baseRecord = null;
        [SerializeField] VerticalLayoutGroup _verticalLayoutForRecords = null;
        [SerializeField] Image _frame = null;
        [SerializeField] RectTransform _recordParent = null;

        public GameObject back;

        public void PlayBillboard()
        {
            _frame.enabled = false;
            if (_recordParent.childCount == 0)
            {
                InitBillboard();
            }
            if (globalData.LastSessionPoint != -1)
            {
                int p = globalData.LastSessionPoint;
                globalData.LastSessionPoint = -1;
                int index = globalData.InsertToBillboard(p);
                if (index < SaveData.TOP_N)
                {
                    back.GetComponent<Button>().interactable = false;
                    StartCoroutine(DelayInsertToPage(p, index));
                }
            }
        }

        private IEnumerator DelayInsertToPage(int point, int index)
        {
            yield return null;
            InsertToPage(point, index);
        }

        private void InitBillboard()
        {
            List<int> points = GlobalData.Instance.TopPoints;
            for (int i = 0; i < SaveData.TOP_N; i++)
            {
                RectTransform newOne = Instantiate(_baseRecord, _recordParent);
                newOne.gameObject.SetActive(true);
                newOne.name = i.ToString();
                newOne.anchoredPosition = _frame.rectTransform.anchoredPosition + Vector2.right * _verticalLayoutForRecords.padding.left;
                SetDataOnRecord(points[i], newOne, i);
                newOne.SetSiblingIndex(i);
            }
        }

        private void BlinkFrame()
        {
            _frame.BlinkAlpha(5, 0.2f);
        }

        private void SetDataOnRecord(int point, RectTransform transform, int index)
        {
            transform.GetChild(0).GetComponent<Text>().text = (index + 1).ToString() + ".";
            transform.GetChild(1).GetComponent<Text>().text = point.ToString();
        }

        private void InsertToPage(int point, int index)
        {
            if (index < 0)
            {
                return;
            }
            _verticalLayoutForRecords.enabled = false;
            _frame.enabled = false;
            if (index < SaveData.TOP_N)
            {
                RectTransform rt = _recordParent.GetChild(0) as RectTransform;
                RectTransform rtNext = _recordParent.GetChild(1) as RectTransform;
                float yOffset = rtNext.anchoredPosition.y - rt.anchoredPosition.y;
                rt = _recordParent.GetChild(_recordParent.childCount - 1) as RectTransform;
                rt.AnchorPos(rt.anchoredPosition + Vector2.up * yOffset, 1, null);

                _frame.rectTransform.anchoredPosition = ((RectTransform)_recordParent.GetChild(index)).anchoredPosition - Vector2.right * _verticalLayoutForRecords.padding.left;
                _frame.rectTransform.SetAsLastSibling();

                int i = index;
                rt = _recordParent.GetChild(index) as RectTransform;
                Vector2 blinkPosition = rt.anchoredPosition;
                for (; i < _recordParent.childCount - 1; i++)
                {
                    rt = _recordParent.GetChild(i) as RectTransform;
                    rtNext = _recordParent.GetChild(i + 1) as RectTransform;
                    Action<RectTransform> onComplete = null;
                    if (i == _recordParent.childCount - 2)
                    {
                        onComplete = (r) =>
                        {
                            OnLastAnchorMoved(r, blinkPosition, index);
                        };
                    }
                    rt.AnchorPos(rtNext.anchoredPosition, 1, onComplete);
                    rt.name = (i + 1).ToString();
                    SetDataOnRecord(globalData.TopPoints[i + 1], rt, i + 1);
                }
            }
        }

        private void OnLastAnchorMoved(RectTransform rt, Vector2 blinkPosition, int insertIndex)
        {
            Debug.LogFormat("last moved one: {0}", rt.GetInstanceID());
            int index = rt.GetSiblingIndex();
            RectTransform blink = rt.parent.GetChild(index + 1) as RectTransform;
            Debug.LogFormat("blink: {0}", blink.GetInstanceID());
            _frame.rectTransform.anchoredPosition = blinkPosition;
            _frame.enabled = true;
            _frame.BlinkAlpha(5, 0.2f);

            rt = _recordParent.GetChild(_recordParent.childCount - 1) as RectTransform;
            rt.anchoredPosition = blinkPosition;
            rt.SetSiblingIndex(insertIndex);
            rt.name = insertIndex.ToString();
            SetDataOnRecord(globalData.TopPoints[insertIndex], rt, insertIndex);

            CanvasRenderer[] cr = rt.GetComponentsInChildren<CanvasRenderer>();
            int i = 0;
            for (; i < cr.Length - 1; i++)
            {
                cr[i].SetAlpha(0);
                cr[i].GetComponent<Graphic>().FadeAlpha(1, 1);
            }
            cr[i].SetAlpha(0);
            cr[i].GetComponent<Graphic>().FadeAlpha(1, 1, EnableBack);
        }

        private void EnableBack()
        {
            back.GetComponent<Button>().interactable = true;
        }
    }
}