using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public static UIMgr single { get; private set; }

    [SerializeField] private CanvasGroup panelSetting;
    [SerializeField] private CanvasGroup panelQuit;
    [SerializeField] private CanvasGroup panelTitle;
    [SerializeField] private CanvasGroup panelLobby;
    [SerializeField] private CanvasGroup panelLogin;

    public static bool _isCreate;


    public void Awake()
    {
        Debug.Log(LoginManager.isLogin);
        single = this;
        if (!LoginManager.isLogin)
        {
            panelLogin.gameObject.SetActive(true);
        }
        else
        {
            panelLogin.gameObject.SetActive(false);
            panelTitle.gameObject.SetActive(true);
        }
    }

    public void OpenSetting()
    {
        FadeManager.Out(panelQuit);
        FadeManager.Out(panelTitle);
        FadeManager.In(panelSetting);
        FadeManager.Out(panelLobby);
        FadeManager.Out(panelLogin);
    }

    public void OpenQuit()
    {
        FadeManager.In(panelQuit);
        FadeManager.Out(panelTitle);
        FadeManager.Out(panelSetting);
        FadeManager.Out(panelLobby);
        FadeManager.Out(panelLogin);
    }

    public void OpenTitle() 
    {
        FadeManager.Out(panelQuit);
        FadeManager.In(panelTitle);
        FadeManager.Out(panelSetting);
        FadeManager.Out(panelLobby);
        FadeManager.Out(panelLogin);
    }

    public void OpenLobby()
    {
        FadeManager.Out(panelQuit);
        FadeManager.In(panelLobby);
        FadeManager.Out(panelSetting);
        FadeManager.Out(panelTitle);
        FadeManager.Out(panelLogin);
    }
}
