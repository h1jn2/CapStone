using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�� ����ϴ� ���

public class CrosshairManager : MonoBehaviour
{
    public Image crosshairImage; // ���ؼ� �̹��� ����
    public Slider sizeSlider;
    public TMP_Dropdown shapeDropdown; // TextMeshPro Dropdown�� ����ϴ� ���
    public TMP_Dropdown colorDropdown; // TextMeshPro Dropdown�� ����ϴ� ���

    void Start()
    {
        // �ʱ� �� ���� �� ������ �߰�
        sizeSlider.onValueChanged.AddListener(OnSizeChanged);
        shapeDropdown.onValueChanged.AddListener(OnShapeChanged);
        colorDropdown.onValueChanged.AddListener(OnColorChanged);

        // �ʱ� �� ����
        OnSizeChanged(sizeSlider.value);
        OnShapeChanged(shapeDropdown.value);
        OnColorChanged(colorDropdown.value);
    }

    void OnSizeChanged(float value)
    {
        // ���ؼ� ũ�� ����
        crosshairImage.rectTransform.localScale = new Vector3(value, value, 1);
    }

    void OnShapeChanged(int index)
    {
        // ���ؼ� ��� ����
        switch (index)
        {
            case 0: // Circle
                crosshairImage.sprite = Resources.Load<Sprite>("Circle");
                break;
            case 1: // Cross
                crosshairImage.sprite = Resources.Load<Sprite>("Cross");
                break;
            case 2: // Dot
                crosshairImage.sprite = Resources.Load<Sprite>("Dot");
                break;
        }
    }

    void OnColorChanged(int index)
    {
        // ���ؼ� ���� ����
        Color newColor = Color.white; // �⺻ ��
        switch (index)
        {
            case 0: // Red
                newColor = Color.red;
                break;
            case 1: // Green
                newColor = Color.green;
                break;
            case 2: // Blue
                newColor = Color.blue;
                break;
            case 3: // Yellow
                newColor = Color.yellow;
                break;
            case 4: // White
                newColor = Color.white;
                break;
            case 5: // Black
                newColor = Color.black;
                break;
        }
        crosshairImage.color = newColor;
    }
}
