using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairMgr : MonoBehaviour
{
    public Image crosshairImage;       // ���ؼ� �̹���
    public Slider sizeSlider;          // ũ�� ���� �����̴�
    public TMP_Dropdown dropdownType;  // ���ؼ� Ÿ�� ��Ӵٿ�
    public TMP_Dropdown dropdownColor; // ���ؼ� ���� ��Ӵٿ�

    public Sprite crossCrosshair;      // ���ڰ� ���ؼ� �̹���
    public Sprite circleCrosshair;     // ���׶�� ���ؼ� �̹���
    public Sprite squareCrosshair;     // �׸� ���ؼ� �̹���

    public Color[] colors = new Color[3];  // ���� �迭

    void Start()
    {
        // ���� �迭 �ʱ�ȭ
        colors[0] = Color.green;  // �ʷϻ�
        colors[1] = Color.blue;   // �Ķ���
        colors[2] = Color.red;    // ������

        // �����̴� ���� ����� ������ OnSliderValueChanged �Լ� ȣ��
        sizeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        // ��Ӵٿ� ���� ����� ������ OnTypeDropdownValueChanged �Լ� ȣ��
        dropdownType.onValueChanged.AddListener(OnTypeDropdownValueChanged);
        // ��Ӵٿ� ���� ����� ������ OnColorDropdownValueChanged �Լ� ȣ��
        dropdownColor.onValueChanged.AddListener(OnColorDropdownValueChanged);

        // �ʱ�ȭ: �����̴� ���� 0.2�� �����ϰ� ���ؼ� ũ�� �� Ÿ�� �ʱ�ȭ
        sizeSlider.value = 0.2f;
        OnSliderValueChanged(sizeSlider.value);
        OnTypeDropdownValueChanged(dropdownType.value);

        // ��Ӵٿ� �ɼ� �ؽ�Ʈ ����
        SetDropdownOptionTexts();
        // ���� ��Ӵٿ� �ɼ� �ؽ�Ʈ ����
        SetColorDropdownOptionTexts();
    }

    void OnSliderValueChanged(float value)
    {
        // �����̴� ���� ���� ���ؼ� �̹��� ũ�� ����
        if (crosshairImage != null)
        {
            crosshairImage.rectTransform.localScale = new Vector3(value, value, 1);
        }
    }

    void OnTypeDropdownValueChanged(int index)
    {
        // ��Ӵٿ� ���� ���� ���ؼ� �̹��� ����
        if (crosshairImage != null)
        {
            switch (index)
            {
                case 0:
                    crosshairImage.sprite = crossCrosshair;
                    break;
                case 1:
                    crosshairImage.sprite = squareCrosshair;
                    break;
                case 2:
                    crosshairImage.sprite = circleCrosshair;
                    break;
                default:
                    break;
            }

          
        }
    }

    void OnColorDropdownValueChanged(int index)
    {
        // ��Ӵٿ� ���� ���� ���ؼ� ���� ����
        if (crosshairImage != null && index >= 0 && index < colors.Length)
        {
            crosshairImage.color = colors[index];

        }
    }

    void SetDropdownOptionTexts()
    {
        // ���ؼ� Ÿ�� ��Ӵٿ� �ɼ� �ؽ�Ʈ ����
        if (dropdownType != null && dropdownType.options != null)
        {
            for (int i = 0; i < dropdownType.options.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        dropdownType.options[i].text = "���ڰ�";  // ���ڰ� ����� ���ؼ�
                        break;
                    case 1:
                        dropdownType.options[i].text = "�簢��";  // �׸� ����� ���ؼ�
                        break;
                    case 2:
                        dropdownType.options[i].text = "��";  // ���׶�� ����� ���ؼ�
                        break;
                    default:
                        break;
                }
            }

            // ��Ӵٿ��� ������Ʈ�Ͽ� ����� �ؽ�Ʈ�� �ݿ�
            dropdownType.RefreshShownValue();
        }
    }

    void SetColorDropdownOptionTexts()
    {
        // ���ؼ� ���� ��Ӵٿ� �ɼ� �ؽ�Ʈ ����
        if (dropdownColor != null && dropdownColor.options != null)
        {
            for (int i = 0; i < dropdownColor.options.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        dropdownColor.options[i].text = "�ʷϻ�";  // �ʷϻ�
                        break;
                    case 1:
                        dropdownColor.options[i].text = "�Ķ���";  // �Ķ�����
                        break;
                    case 2:
                        dropdownColor.options[i].text = "������";  // ������
                        break;
                    default:
                        break;
                }
            }

            // ��Ӵٿ��� ������Ʈ�Ͽ� ����� �ؽ�Ʈ�� �ݿ�
            dropdownColor.RefreshShownValue();
        }
    }
}

