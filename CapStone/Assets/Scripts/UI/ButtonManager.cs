using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 버튼 텍스트 (TMPro)
    public TMP_Text buttonText;

    // 해당 버튼에 대한 이미지 배열
    public Image[] buttonImages;

    // 변경할 색상의 RGB 값
    public float hoverColorR = 0.8f;
    public float hoverColorG = 0.8f;
    public float hoverColorB = 0.8f;

    // 기존의 색상
    private Color originalTextColor;
    private Color[] originalImageColors;

    void Start()
    {
        // 버튼의 기존 색상 저장
        originalTextColor = buttonText.color;

        // 해당 버튼에 대한 이미지 배열의 각 요소의 기존 색상 저장
        originalImageColors = new Color[buttonImages.Length];
        for (int i = 0; i < buttonImages.Length; i++)
        {
            originalImageColors[i] = buttonImages[i].color;
        }
    }

    // 마우스가 버튼에 진입했을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 변경할 RGB 값을 사용하여 새로운 텍스트 색상 생성
        Color newTextColor = new Color(hoverColorR, hoverColorG, hoverColorB);

        // 버튼 텍스트 색상 변경
        buttonText.color = newTextColor;

        // 해당 버튼에 대한 이미지 배열의 모든 이미지 활성화
        foreach (Image buttonImage in buttonImages)
        {
            buttonImage.gameObject.SetActive(true);
        }
    }

    // 마우스가 버튼에서 나갔을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        // 버튼 텍스트 색상을 원래 색상으로 변경
        buttonText.color = originalTextColor;

        // 해당 버튼에 대한 이미지 배열의 모든 이미지 비활성화
        foreach (Image buttonImage in buttonImages)
        {
            buttonImage.gameObject.SetActive(false);
        }
    }
}
