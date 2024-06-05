using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMgr : MonoBehaviour
{
    public static SettingMgr single { get; private set; }

    [SerializeField] private CanvasGroup panelSetting;
    [SerializeField] private CanvasGroup panelQuit;
    [SerializeField] private CanvasGroup panelTitle;

    public bool gamePaused { get; private set; } = false; // 게임이 멈췄는지 여부를 나타내는 변수

    public void Awake()
    {
        single = this;
    }

    void Update()
    {
        // ESC 키를 누르면 게임을 일시 정지하고 패널을 엽니다.
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
        Time.timeScale = 0f; // 게임 시간을 정지합니다.
        panelTitle.gameObject.SetActive(true); // 타이틀 패널을 활성화합니다.
    }

    void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1f; // 게임 시간을 다시 시작합니다.
        panelTitle.gameObject.SetActive(false); // 타이틀 패널을 비활성화합니다.
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
        Debug.Log("게임종료");
    }
}
