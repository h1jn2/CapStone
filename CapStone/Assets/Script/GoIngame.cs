using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoIngame : MonoBehaviour
{
    public Image imageToShow; // ��ȯ�� �̹���
    public string sceneToLoad; // ��ȯ�� ���� �̸�
    public float delayBeforeSceneLoad = 0.5f; // �� ��ȯ �� ��� �ð�
    public float fadeDuration = 0.1f; // ���̵��� �ð�

    private Button button; // Ŭ���� ��ư

    void Start()
    {
        // ��ư ã��
        button = GetComponent<Button>();

        // ��ư Ŭ�� �̺�Ʈ�� �Լ� ����
        button.onClick.AddListener(OnButtonClick);
    }

    // ��ư Ŭ�� �� ������ �Լ�
    void OnButtonClick()
    {
        // �̹����� Ȱ��ȭ�Ͽ� ������
        imageToShow.gameObject.SetActive(true);

        // ���̵��� ȿ�� �ڷ�ƾ ����
        StartCoroutine(FadeInImage(imageToShow, fadeDuration));

        // ���� �ð��� ���� �� �ٸ� ������ ��ȯ
        StartCoroutine(LoadSceneWithDelay(sceneToLoad, delayBeforeSceneLoad + fadeDuration)); // ���̵��� �ð��� ����Ͽ� delayBeforeSceneLoad�� ���̵��� �ð��� ���մϴ�.
    }

    // �̹����� ���̵����ϴ� �Լ�
    IEnumerator FadeInImage(Image image, float duration)
    {
        float currentTime = 0;
        Color startColor = new Color(image.color.r, image.color.g, image.color.b, 0); // ���� ���İ��� 0���� �����Ͽ� ������ ���·� �����մϴ�.
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1); // ���� ���İ��� 1�� �����Ͽ� ������ �ִ�� ����ϴ�.

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, currentTime / duration);
            yield return null;
        }
    }

    // ���� �ð��� ���� �� ���� ��ȯ�ϴ� �Լ�
    IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        // ���
        yield return new WaitForSeconds(delay);

        // �� ��ȯ
        SceneManager.LoadScene(sceneName);
    }
}
