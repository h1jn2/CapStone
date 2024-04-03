using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoIngame : MonoBehaviour
{
    public Image imageToShow; // 전환될 이미지
    public string sceneToLoad; // 전환할 씬의 이름
    public float delayBeforeSceneLoad = 0.5f; // 씬 전환 전 대기 시간
    public float fadeDuration = 0.1f; // 페이드인 시간

    private Button button; // 클릭할 버튼

    void Start()
    {
        // 버튼 찾기
        button = GetComponent<Button>();

        // 버튼 클릭 이벤트에 함수 연결
        button.onClick.AddListener(OnButtonClick);
    }

    // 버튼 클릭 시 실행할 함수
    void OnButtonClick()
    {
        // 이미지를 활성화하여 보여줌
        imageToShow.gameObject.SetActive(true);

        // 페이드인 효과 코루틴 시작
        StartCoroutine(FadeInImage(imageToShow, fadeDuration));

        // 일정 시간이 지난 후 다른 씬으로 전환
        StartCoroutine(LoadSceneWithDelay(sceneToLoad, delayBeforeSceneLoad + fadeDuration)); // 페이드인 시간을 고려하여 delayBeforeSceneLoad에 페이드인 시간을 더합니다.
    }

    // 이미지를 페이드인하는 함수
    IEnumerator FadeInImage(Image image, float duration)
    {
        float currentTime = 0;
        Color startColor = new Color(image.color.r, image.color.g, image.color.b, 0); // 시작 알파값을 0으로 설정하여 투명한 상태로 시작합니다.
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1); // 최종 알파값을 1로 설정하여 투명도를 최대로 만듭니다.

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;
        }
    }

    // 일정 시간이 지난 후 씬을 전환하는 함수
    IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        // 대기
        yield return new WaitForSeconds(delay);

        // 씬 전환
        SceneManager.LoadScene(sceneName);
    }
}
