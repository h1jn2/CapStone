using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하는 경우

public class CrosshairManager : MonoBehaviour
{
    public Image crosshairImage; // 조준선 이미지 참조
    public Slider sizeSlider;
    public TMP_Dropdown shapeDropdown; // TextMeshPro Dropdown을 사용하는 경우
    public TMP_Dropdown colorDropdown; // TextMeshPro Dropdown을 사용하는 경우

    void Start()
    {
        // 초기 값 설정 및 리스너 추가
        sizeSlider.onValueChanged.AddListener(OnSizeChanged);
        shapeDropdown.onValueChanged.AddListener(OnShapeChanged);
        colorDropdown.onValueChanged.AddListener(OnColorChanged);

        // 초기 값 적용
        OnSizeChanged(sizeSlider.value);
        OnShapeChanged(shapeDropdown.value);
        OnColorChanged(colorDropdown.value);
    }

    void OnSizeChanged(float value)
    {
        // 조준선 크기 변경
        crosshairImage.rectTransform.localScale = new Vector3(value, value, 1);
    }

    void OnShapeChanged(int index)
    {
        // 조준선 모양 변경
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
        // 조준선 색상 변경
        Color newColor = Color.white; // 기본 값
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
