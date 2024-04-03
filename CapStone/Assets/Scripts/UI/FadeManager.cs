using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;

public class FadeManager : MonoBehaviour
{

    private static FadeManager single;

    //private CanvasGroup currentPanel;
    //private CanvasGroup nextPanel;
    //private float fadeDuration = 0.5f;

    //private bool isTransitioning = false;
    private Coroutine coroutineIn;
    private Coroutine coroutineOut;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        single = this;
    }

    public static void Call(CanvasGroup current, CanvasGroup next, float duration = 0.5f)
    {
        FadeManager.Out(current);
        FadeManager.In(next);

    }

    public static void In(CanvasGroup target, float wait = 0.5f, float duratoin = 0.5f)
    {
        single.coroutineIn = single.StartCoroutine(single.FadeIn(target, wait, duratoin));
    }

    public static void Out(CanvasGroup target, float duration = 0.5f)
    {
        if (!target.gameObject.activeSelf)
            return;

        single.coroutineOut = single.StartCoroutine(single.FadeOut(target, duration));
    }


    private IEnumerator FadeOut(CanvasGroup target, float duration = 0.5f)
    {
        float currentTime = 0f;
        float startAlpha = target.alpha;
        float endAlpha = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            target.alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            yield return null;
        }

        target.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn(CanvasGroup target, float wait = 0.5f, float duration = 0.5f)
    {
        target.gameObject.SetActive(false);
        yield return new WaitForSeconds(wait);

        target.gameObject.SetActive(true);
        target.alpha = 0f;

        float currentTime = 0f;
        float startAlpha = target.alpha;
        float endAlpha = 0f;

        currentTime = 0f;
        endAlpha = 1f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            target.alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            yield return null;
        }
    }

}
