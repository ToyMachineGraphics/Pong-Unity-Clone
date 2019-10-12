using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PongClone
{
    public static class UIEffect
    {
        public static void FadeAlpha(this Graphic body, float alpha, float duration, Action onComplete = null)
        {
            body.StopAllCoroutines();
            body.StartCoroutine(FadeAlphaTask(body, alpha, duration, onComplete));
        }

        private static IEnumerator FadeAlphaTask(Graphic body, float alpha, float duration, Action onComplete)
        {
            body.CrossFadeAlpha(alpha, duration, false);
            yield return new WaitUntil(() => body.canvasRenderer.GetAlpha() == 1);
            onComplete?.Invoke();
        }

        public static void BlinkAlpha(this Graphic body, int times, float interval, float alpha = 0.0f)
        {
            body.StopAllCoroutines();
            body.StartCoroutine(AlphaBlinkTask(body, times, interval, alpha));
        }

        private static IEnumerator AlphaBlinkTask(Graphic body, int times, float interval, float alpha)
        {
            float a = body.color.a;
            int time = 0;
            WaitForSeconds wait = new WaitForSeconds(interval / 2);
            while (time++ < times)
            {
                body.CrossFadeAlpha(alpha, 0, false);
                yield return wait;
                body.CrossFadeAlpha(a, 0, false);
                yield return wait;
            }
            yield return null;
        }

        public static void AnchorPos(this RectTransform transform, Vector2 to, float duration, Action<RectTransform> onComplete = null)
        {
            MonoBehaviour behaviour = transform.GetComponent<MonoBehaviour>();
            MonoBehaviour destroy = null;
            if (!behaviour)
            {
                behaviour = transform.gameObject.AddComponent<CoroutineHost>();
                destroy = behaviour;
            }
            behaviour.StopAllCoroutines();
            behaviour.StartCoroutine(AnchorPosTask(transform, to, duration, destroy, onComplete));
        }

        private static IEnumerator AnchorPosTask(RectTransform transform, Vector2 to, float duration, MonoBehaviour destroy, Action<RectTransform> onComplete)
        {
            float time = 0;
            RectTransform rt = transform;
            Vector2 origin = rt.anchoredPosition;
            while (time < duration)
            {
                time += Time.deltaTime;
                rt.anchoredPosition = Vector2.Lerp(origin, to, time / duration);
                yield return null;
            }
            rt.anchoredPosition = to;
            UnityEngine.Object.Destroy(destroy);
            onComplete?.Invoke(transform);
        }
    }
}