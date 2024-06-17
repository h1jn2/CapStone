using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairMgr : MonoBehaviour
{
    public Image crosshairImage;       // 조준선 이미지
    public Slider sizeSlider;          // 크기 조절 슬라이더
    public TMP_Dropdown dropdownType;  // 조준선 타입 드롭다운
    public TMP_Dropdown dropdownColor; // 조준선 색상 드롭다운

    public Sprite crossCrosshair;      // 십자가 조준선 이미지
    public Sprite circleCrosshair;     // 동그라미 조준선 이미지
    public Sprite squareCrosshair;     // 네모 조준선 이미지

    public Color[] colors = new Color[3];  // 색상 배열

    void Start()
    {
        // 색상 배열 초기화
        colors[0] = Color.green;  // 초록색
        colors[1] = Color.blue;   // 파란색
        colors[2] = Color.red;    // 빨간색

        // 슬라이더 값이 변경될 때마다 OnSliderValueChanged 함수 호출
        sizeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        // 드롭다운 값이 변경될 때마다 OnTypeDropdownValueChanged 함수 호출
        dropdownType.onValueChanged.AddListener(OnTypeDropdownValueChanged);
        // 드롭다운 값이 변경될 때마다 OnColorDropdownValueChanged 함수 호출
        dropdownColor.onValueChanged.AddListener(OnColorDropdownValueChanged);

        // 초기화: 슬라이더 값을 0.2로 설정하고 조준선 크기 및 타입 초기화
        sizeSlider.value = 0.2f;
        OnSliderValueChanged(sizeSlider.value);
        OnTypeDropdownValueChanged(dropdownType.value);

        // 드롭다운 옵션 텍스트 설정
        SetDropdownOptionTexts();
        // 색상 드롭다운 옵션 텍스트 설정
        SetColorDropdownOptionTexts();
    }

    void OnSliderValueChanged(float value)
    {
        // 슬라이더 값에 따라 조준선 이미지 크기 조절
        if (crosshairImage != null)
        {
            crosshairImage.rectTransform.localScale = new Vector3(value, value, 1);
        }
    }

    void OnTypeDropdownValueChanged(int index)
    {
        // 드롭다운 값에 따라 조준선 이미지 변경
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
        // 드롭다운 값에 따라 조준선 색상 변경
        if (crosshairImage != null && index >= 0 && index < colors.Length)
        {
            crosshairImage.color = colors[index];

        }
    }

    void SetDropdownOptionTexts()
    {
        // 조준선 타입 드롭다운 옵션 텍스트 설정
        if (dropdownType != null && dropdownType.options != null)
        {
            for (int i = 0; i < dropdownType.options.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        dropdownType.options[i].text = "십자가";  // 십자가 모양의 조준선
                        break;
                    case 1:
                        dropdownType.options[i].text = "사각형";  // 네모 모양의 조준선
                        break;
                    case 2:
                        dropdownType.options[i].text = "점";  // 동그라미 모양의 조준선
                        break;
                    default:
                        break;
                }
            }

            // 드롭다운을 업데이트하여 변경된 텍스트를 반영
            dropdownType.RefreshShownValue();
        }
    }

    void SetColorDropdownOptionTexts()
    {
        // 조준선 색상 드롭다운 옵션 텍스트 설정
        if (dropdownColor != null && dropdownColor.options != null)
        {
            for (int i = 0; i < dropdownColor.options.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        dropdownColor.options[i].text = "초록색";  // 초록색
                        break;
                    case 1:
                        dropdownColor.options[i].text = "파란색";  // 파란색ㅂ
                        break;
                    case 2:
                        dropdownColor.options[i].text = "빨간색";  // 빨간색
                        break;
                    default:
                        break;
                }
            }

            // 드롭다운을 업데이트하여 변경된 텍스트를 반영
            dropdownColor.RefreshShownValue();
        }
    }
}

