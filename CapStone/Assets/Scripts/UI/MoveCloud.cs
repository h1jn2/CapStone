using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveCloud : MonoBehaviour
{
    public float speed = 15f; // �̹����� �̵� �ӵ�

    private RectTransform rectTransform; // �̹����� RectTransform ������Ʈ

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // �̹����� RectTransform ������Ʈ ��������
    }

    void Update()
    {
        // �̹����� ���������� �̵�
        rectTransform.Translate(Vector3.right * speed * Time.deltaTime);

        // �̹����� ĵ������ �����
        if (rectTransform.anchoredPosition.x > Screen.width / 2 + rectTransform.rect.width / 2)
        {
            // �̹����� ĵ���� ���� ������ �̵�
            rectTransform.anchoredPosition = new Vector2(-Screen.width / 2 - rectTransform.rect.width / 2, rectTransform.anchoredPosition.y);
        }
    }
}
