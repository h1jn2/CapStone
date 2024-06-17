using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameClear1 : MonoBehaviour
{
    public static GameClear1 instance = null;
    public GameObject clearImage; // Ŭ���� �̹����� �Ҵ�
    public GameObject loseImage;  // �й� �̹����� �Ҵ�
    public float fadeDuration = 1.0f; // ���̵��� ���� �ð�
    public Camera mainCamera; // ���� ���� ī�޶�
    public Button testClearButton; // �׽�Ʈ Ŭ���� ��ư
    public Button testLoseButton;  // �׽�Ʈ �й� ��ư

    private CanvasGroup clearCanvasGroup;
    private CanvasGroup loseCanvasGroup;
    

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        if (instance != this)
        {
            Destroy(this.gameObject); // 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
        }
        
        // clearImage���� CanvasGroup�� ������. ������ �߰�.
        clearCanvasGroup = clearImage.GetComponent<CanvasGroup>();
        if (clearCanvasGroup == null)
        {
            clearCanvasGroup = clearImage.AddComponent<CanvasGroup>();
        }

        // loseImage���� CanvasGroup�� ������. ������ �߰�.
        loseCanvasGroup = loseImage.GetComponent<CanvasGroup>();
        if (loseCanvasGroup == null)
        {
            loseCanvasGroup = loseImage.AddComponent<CanvasGroup>();
        }

        // ���� �� �̹����� �����ϰ� ����
        clearCanvasGroup.alpha = 0f;
        clearImage.SetActive(false);
        loseCanvasGroup.alpha = 0f;
        loseImage.SetActive(false);

        // �׽�Ʈ ��ư�� Ŭ�� ������ �߰�
        if (testClearButton != null)
        {
            testClearButton.onClick.AddListener(OnGameClear);
        }
        if (testLoseButton != null)
        {
            testLoseButton.onClick.AddListener(OnGameLose);
        }
    }

    public void OnDefeat()
    {
        if (PhotonManager.instance == null)
            return;
        PhotonManager.instance.DisconnectRoom();
    }

    public void OnGameClear()
    {
        // ���� �Ͻ� ����
        // 해당 코드 활성화시 포톤 leaveRoom 실행 안됨!!!!!!!!!!!!
        //Time.timeScale = 0f;

        // Ŭ���� �̹����� Ȱ��ȭ�ϰ� ���̵��� �ڷ�ƾ ����
        clearImage.SetActive(true);
        StartCoroutine(FadeInAndZoomCamera(clearCanvasGroup));
    }

    public void OnGameLose()
    {
        // ���� �Ͻ� ����
        // 해당 코드 활성화시 포톤 leaveRoom 실행 안됨!!!!!!!!!!!!
        //Time.timeScale = 0f;

        // �й� �̹����� Ȱ��ȭ�ϰ� ���̵��� �ڷ�ƾ ����
        loseImage.SetActive(true);
        StartCoroutine(FadeInAndZoomCamera(loseCanvasGroup));
    }

    IEnumerator FadeInAndZoomCamera(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        float initialCameraSize = mainCamera.orthographicSize;
        float targetCameraSize = initialCameraSize * 1.5f; // ���ϴ� Ȯ�� ������ ����

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // ������ �Ͻ� ������ ���ȿ��� �ð��� �帣���� Time.unscaledDeltaTime ���
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            // ī�޶� Ȯ��
            float t = elapsedTime / fadeDuration;
            mainCamera.orthographicSize = Mathf.Lerp(initialCameraSize, targetCameraSize, t);

            yield return null;
        }

        // ���̵����� �Ϸ�Ǹ� ������ �������ϰ� ����
        canvasGroup.alpha = 1f;
        mainCamera.orthographicSize = targetCameraSize; // ���� Ȯ�밪 ����
    }
}
