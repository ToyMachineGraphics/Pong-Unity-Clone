using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameplayUI : MonoBehaviour
{
    [SerializeField] protected Text[] _points;
    [SerializeField] protected AnimationCurve _shineCurve;
    [SerializeField] protected CanvasGroup _matchEnding;

    protected IEnumerator PointShine(int id, Action onComplete)
    {
        Color c = _points[id].color;
        float alpha = _points[id].color.a;
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            float a = alpha + _shineCurve.Evaluate(time);
            _points[id].color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        _points[id].color = new Color(c.r, c.g, c.b, alpha);
        onComplete?.Invoke();
    }

    public void EndMatch()
    {
        StartCoroutine(FadeInMatchEndingUI());
        Debug.Log("UI EndMatch");
    }

    private IEnumerator FadeInMatchEndingUI()
    {
        yield return new WaitForSeconds(1);
        _matchEnding.gameObject.SetActive(true);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 0.75f;
            _matchEnding.alpha = t;
            yield return null;
        }
    }
}
