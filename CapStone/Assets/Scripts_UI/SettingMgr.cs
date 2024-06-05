using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMgr : MonoBehaviour
{
    public static SettingMgr single { get; private set; }

    [SerializeField] private CanvasGroup panelSetting;
    [SerializeField] private CanvasGroup panelQuit;
    [SerializeField] private CanvasGroup panelTitle;

    public bool gamePaused { get; private set; } = false; // ������ ������� ���θ� ��Ÿ���� ����

    public void Awake()
    {
        single = this;
    }

    void Update()
    {
        // ESC Ű�� ������ ������ �Ͻ� �����ϰ� �г��� ���ϴ�.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0f; // ���� �ð��� �����մϴ�.
        panelTitle.gameObject.SetActive(true); // Ÿ��Ʋ �г��� Ȱ��ȭ�մϴ�.
    }

    void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1f; // ���� �ð��� �ٽ� �����մϴ�.
        panelTitle.gameObject.SetActive(false); // Ÿ��Ʋ �г��� ��Ȱ��ȭ�մϴ�.
    }

    public void OpenSetting()
    {
        FadeManager.In(panelSetting);
        FadeManager.Out(panelTitle);
    }

    public void OpenQuit()
    {
        FadeManager.In(panelQuit);
        FadeManager.Out(panelTitle);
    }

    public void OutSetting()
    {
        FadeManager.Out(panelSetting);
        FadeManager.In(panelTitle);
    }

    public void OutQuit()
    {
        FadeManager.Out(panelQuit);
        FadeManager.In(panelTitle);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("��������");
    }
}
