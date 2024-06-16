using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameClear1 : MonoBehaviour
{
    public GameObject clearImage; // 클리어 이미지를 할당
    public GameObject loseImage;  // 패배 이미지를 할당
    public float fadeDuration = 1.0f; // 페이드인 지속 시간
    public Camera mainCamera; // 게임 메인 카메라
    public Button testClearButton; // 테스트 클리어 버튼
    public Button testLoseButton;  // 테스트 패배 버튼

    private CanvasGroup clearCanvasGroup;
    private CanvasGroup loseCanvasGroup;

    void Start()
    {
        // clearImage에서 CanvasGroup을 가져옴. 없으면 추가.
        clearCanvasGroup = clearImage.GetComponent<CanvasGroup>();
        if (clearCanvasGroup == null)
        {
            clearCanvasGroup = clearImage.AddComponent<CanvasGroup>();
        }

        // loseImage에서 CanvasGroup을 가져옴. 없으면 추가.
        loseCanvasGroup = loseImage.GetComponent<CanvasGroup>();
        if (loseCanvasGroup == null)
        {
            loseCanvasGroup = loseImage.AddComponent<CanvasGroup>();
        }

        // 시작 시 이미지를 투명하게 설정
        clearCanvasGroup.alpha = 0f;
        clearImage.SetActive(false);
        loseCanvasGroup.alpha = 0f;
        loseImage.SetActive(false);

        // 테스트 버튼에 클릭 리스너 추가
        if (testClearButton != null)
        {
            testClearButton.onClick.AddListener(OnGameClear);
        }
        if (testLoseButton != null)
        {
            testLoseButton.onClick.AddListener(OnGameLose);
        }
    }

    public void OnGameClear()
    {
        // 게임 일시 정지
        Time.timeScale = 0f;

        // 클리어 이미지를 활성화하고 페이드인 코루틴 시작
        clearImage.SetActive(true);
        StartCoroutine(FadeInAndZoomCamera(clearCanvasGroup));
    }

    public void OnGameLose()
    {
        // 게임 일시 정지
        Time.timeScale = 0f;

        // 패배 이미지를 활성화하고 페이드인 코루틴 시작
        loseImage.SetActive(true);
        StartCoroutine(FadeInAndZoomCamera(loseCanvasGroup));
    }

    IEnumerator FadeInAndZoomCamera(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        float initialCameraSize = mainCamera.orthographicSize;
        float targetCameraSize = initialCameraSize * 1.5f; // 원하는 확대 배율로 조정

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // 게임이 일시 정지된 동안에도 시간이 흐르도록 Time.unscaledDeltaTime 사용
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            // 카메라 확대
            float t = elapsedTime / fadeDuration;
            mainCamera.orthographicSize = Mathf.Lerp(initialCameraSize, targetCameraSize, t);

            yield return null;
        }

        // 페이드인이 완료되면 완전히 불투명하게 설정
        canvasGroup.alpha = 1f;
        mainCamera.orthographicSize = targetCameraSize; // 최종 확대값 설정
    }
}
