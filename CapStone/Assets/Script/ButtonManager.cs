using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ��ư �ؽ�Ʈ (TMPro)
    public TMP_Text buttonText;

    // �ش� ��ư�� ���� �̹��� �迭
    public Image[] buttonImages;

    // ������ ������ RGB ��
    public float hoverColorR = 0.8f;
    public float hoverColorG = 0.8f;
    public float hoverColorB = 0.8f;

    // ������ ����
    private Color originalTextColor;
    private Color[] originalImageColors;

    void Start()
    {
        // ��ư�� ���� ���� ����
        originalTextColor = buttonText.color;

        // �ش� ��ư�� ���� �̹��� �迭�� �� ����� ���� ���� ����
        originalImageColors = new Color[buttonImages.Length];
        for (int i = 0; i < buttonImages.Length; i++)
        {
            originalImageColors[i] = buttonImages[i].color;
        }
    }

    // ���콺�� ��ư�� �������� �� ȣ��Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ RGB ���� ����Ͽ� ���ο� �ؽ�Ʈ ���� ����
        Color newTextColor = new Color(hoverColorR, hoverColorG, hoverColorB);

        // ��ư �ؽ�Ʈ ���� ����
        buttonText.color = newTextColor;

        // �ش� ��ư�� ���� �̹��� �迭�� ��� �̹��� Ȱ��ȭ
        foreach (Image buttonImage in buttonImages)
        {
            buttonImage.gameObject.SetActive(true);
        }
    }

    // ���콺�� ��ư���� ������ �� ȣ��Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        // ��ư �ؽ�Ʈ ������ ���� �������� ����
        buttonText.color = originalTextColor;

        // �ش� ��ư�� ���� �̹��� �迭�� ��� �̹��� ��Ȱ��ȭ
        foreach (Image buttonImage in buttonImages)
        {
            buttonImage.gameObject.SetActive(false);
        }
    }
}
