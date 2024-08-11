using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;

public class FadeManager : MonoBehaviour
{

    private static FadeManager single;

    private Coroutine coroutineIn;
    private Coroutine coroutineOut;
    static private Dictionary<CanvasGroup, Coroutine> dictCoroutineOneTime = new Dictionary<CanvasGroup, Coroutine>();

    private void Awake()
    {if (single == null)
        {
            single = this;
            DontDestroyOnLoad(this);    
        }
        else
        {
            if (single != this)
            {
                Destroy(this.gameObject); // 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
            }
        }
    }

    public static void Call(CanvasGroup current, CanvasGroup next, float duration = 0.5f)
    {
        FadeManager.Out(current);
        FadeManager.In(next, duration); // 페이드인 시간을 전달

    }

    public static void In(CanvasGroup target, float duration = 0.5f, float wait = 0)
    {
        single.coroutineIn = single.StartCoroutine(single.FadeIn(target, duration));
    }

    public static void InOneTime(CanvasGroup target, float duration = 0.5f, float wait = 0)
    {
        Coroutine coroutine = null;
        if(dictCoroutineOneTime.TryGetValue(target, out coroutine))
        {
            single.StopCoroutine(coroutine);
            dictCoroutineOneTime.Remove(target);
        }

        dictCoroutineOneTime.Add(target, single.StartCoroutine(single.FadeIn(target, duration, wait)));

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
            currentTime += Time.unscaledDeltaTime;
            target.alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            yield return null;
        }

        target.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn(CanvasGroup target, float duration = 0.5f, float wait = 0)
    {
        target.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(wait); // 기다리는 시간을 조절하려면 WaitForSeconds 대신에 yield return null을 사용

        target.gameObject.SetActive(true);
        target.alpha = 0f;

        float currentTime = 0f;
        float startAlpha = target.alpha;
        float endAlpha = 0f;

        currentTime = 0f;
        endAlpha = 1f;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            target.alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            yield return null;
        }
    }

}
