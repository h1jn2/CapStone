using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] playerButtons; // �÷��̾� ��ư��
    public GameObject[] playerImages; // �÷��̾� �̹�����
    public GameObject[] playerLines; // �÷��̾� ���ε�

    private Material grayscaleMaterial;
    private Color originalLineColor;
    private bool[] isMouseOverPlayer; // �� �÷��̾� ��ư ���� ���콺�� �ִ��� ����

    void Start()
    {
        // ��� ���̴��� �����ϱ� ���� ��Ƽ���� ����
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

        // �ʱ�ȭ
        isMouseOverPlayer = new bool[playerButtons.Length];
        originalLineColor = playerLines[0].GetComponent<Image>().color;

        // ��� �̹����� ������ ������� ����
        SetGrayscale(true);

        // �� ��ư�� �̺�Ʈ Ʈ���� �߰�
        for (int i = 0; i < playerButtons.Length; i++)
        {
            int playerNumber = i; // �÷��̾� ��ȣ�� 0���� ����
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
        // �ش� �÷��̾� �̹����� ��� ȿ�� ����
        SetGrayscale(false, playerNumber);

        // �ش� �÷��̾� ���� ������� ����
        SetLineColor(playerNumber, Color.white);

        // �ش� �÷��̾� ���� ��¦�̴� ȿ�� ����
        StartCoroutine(BlinkLine(playerNumber));
    }

    public void OnPointerExit(int playerNumber)
    {
        // �ش� �÷��̾� �̹����� ��� ȿ�� �ٽ� ����
        SetGrayscale(true, playerNumber);

        // �ش� �÷��̾� ���� ��¦�̴� ȿ�� ����
        StopCoroutine(BlinkLine(playerNumber));
        SetLineColor(playerNumber, originalLineColor); // ��¦�� ���� �� ���� �������� ����
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
            // ���� ������ ���� ��¦�� ȿ��
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
