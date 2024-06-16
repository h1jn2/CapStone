using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] playerButtons; // 플레이어 버튼들
    public GameObject[] playerImages; // 플레이어 이미지들
    public GameObject[] playerLines; // 플레이어 라인들

    private Material grayscaleMaterial;
    private Color originalLineColor;
    private bool[] isMouseOverPlayer; // 각 플레이어 버튼 위에 마우스가 있는지 여부

    void Start()
    {
        // 흑백 셰이더를 적용하기 위한 머티리얼 설정
        Shader grayscaleShader = Shader.Find("Custom/Grayscale");
        if (grayscaleShader != null)
        {
            grayscaleMaterial = new Material(grayscaleShader);
        }
        else
        {
            Debug.LogError("Grayscale shader not found");
            return;
        }

        // 초기화
        isMouseOverPlayer = new bool[playerButtons.Length];
        originalLineColor = playerLines[0].GetComponent<Image>().color;

        // 모든 이미지와 라인을 흑백으로 설정
        SetGrayscale(true);

        // 각 버튼에 이벤트 트리거 추가
        for (int i = 0; i < playerButtons.Length; i++)
        {
            int playerNumber = i; // 플레이어 번호는 0부터 시작
            AddEventTrigger(playerButtons[i], EventTriggerType.PointerEnter, () => OnPointerEnter(playerNumber));
            AddEventTrigger(playerButtons[i], EventTriggerType.PointerExit, () => OnPointerExit(playerNumber));
        }
    }

    private void AddEventTrigger(GameObject target, EventTriggerType eventType, System.Action callback)
    {
        if (target != null)
        {
            EventTrigger trigger = target.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = target.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener((eventData) => callback());
            trigger.triggers.Add(entry);
        }
        else
        {
            Debug.LogError("Target GameObject is not assigned.");
        }
    }

    public void OnPointerEnter(int playerNumber)
    {
        // 해당 플레이어 이미지의 흑백 효과 해제
        SetGrayscale(false, playerNumber);

        // 해당 플레이어 라인 흰색으로 변경
        SetLineColor(playerNumber, Color.white);

        // 해당 플레이어 라인 반짝이는 효과 시작
        StartCoroutine(BlinkLine(playerNumber));
    }

    public void OnPointerExit(int playerNumber)
    {
        // 해당 플레이어 이미지의 흑백 효과 다시 적용
        SetGrayscale(true, playerNumber);

        // 해당 플레이어 라인 반짝이는 효과 중지
        StopCoroutine(BlinkLine(playerNumber));
        SetLineColor(playerNumber, originalLineColor); // 반짝임 중지 후 원래 색상으로 설정
    }

    private void SetGrayscale(bool grayscale, int playerNumber = -1)
    {
        for (int i = 0; i < playerImages.Length; i++)
        {
            Image image = playerImages[i].GetComponent<Image>();
            if (image != null)
            {
                Material mat = grayscale || i != playerNumber ? grayscaleMaterial : null;
                image.material = mat;
            }
        }
    }

    private void SetLineColor(int playerNumber, Color color)
    {
        Image lineImage = playerLines[playerNumber].GetComponent<Image>();
        lineImage.color = color;
    }

    private IEnumerator BlinkLine(int playerNumber)
    {
        float blinkSpeed = 1.0f;
        Image lineImage = playerLines[playerNumber].GetComponent<Image>();

        while (true)
        {
            // 투명도 조절을 통한 반짝임 효과
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            SetLineAlpha(playerNumber, alpha);

            yield return null;
        }
    }

    private void SetLineAlpha(int playerNumber, float alpha)
    {
        Image lineImage = playerLines[playerNumber].GetComponent<Image>();
        Color color = lineImage.color;
        color.a = alpha;
        lineImage.color = color;
    }
}
