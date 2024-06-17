using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public static UIMgr single { get; private set; }

    [SerializeField] private CanvasGroup panelSetting;
    [SerializeField] private CanvasGroup panelQuit;
    [SerializeField] private CanvasGroup panelTitle;
    [SerializeField] private CanvasGroup panelLobby;
    [SerializeField] private CanvasGroup panelLogin;
    [SerializeField] private CanvasGroup panelLoading;
    [SerializeField] private CanvasGroup objLoadingBottom;
    [SerializeField] private CanvasGroup panelRoom;
    [SerializeField] private CanvasGroup playerselect;

    [SerializeField] private CanvasGroup CreateCode;
    [SerializeField] private CanvasGroup ToRoom;

    [SerializeField] private Button closeButtonCreateCode;
    [SerializeField] private Button closeButtonToRoom;

    public void Awake()
    {
        single = this;

        // ��ư �̺�Ʈ ����
        closeButtonCreateCode.onClick.AddListener(CloseCreateCodePopup);
        closeButtonToRoom.onClick.AddListener(CloseToRoomPopup);

        if (LoginManager.isLogin)
        {
            panelLogin.gameObject.SetActive(false);
            panelTitle.gameObject.SetActive(true);
        }
    }

    public void OpenLoading()
    {
        FadeManager.In(panelLoading, 2);
        //FadeManager.InOneTime(objLoadingBottom, wait: 1);
        FadeManager.Out(panelQuit);
        FadeManager.Out(panelRoom);
        FadeManager.Out(panelTitle);
        FadeManager.Out(panelLobby);
        FadeManager.Out(panelLogin);
        FadeManager.Out(panelSetting);
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
        FadeManager.Out(playerselect);
    }

    public void OpenLobby()
    {
        FadeManager.Out(panelQuit);
        FadeManager.In(panelLobby);
        FadeManager.In(playerselect);
        FadeManager.Out(panelSetting);
        FadeManager.Out(panelTitle);
        FadeManager.Out(panelLogin);
        
    }

    public void OpenCreateCodePopup()
    {
        FadeManager.In(CreateCode);
    }

    public void CloseCreateCodePopup()
    {
        FadeManager.Out(CreateCode);
    }

    public void OpenToRoomPopup()
    {
        FadeManager.In(ToRoom);
    }

    public void CloseToRoomPopup()
    {
        FadeManager.Out(ToRoom);
    }

    public void CloseToPlayerSelect()
    {
        FadeManager.Out(playerselect);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("��������");
    }
}
