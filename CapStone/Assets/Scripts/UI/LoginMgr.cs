using System.Collections;
using Michsky.UI.Dark;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LoginMgr : MonoBehaviour
{
    public TMP_InputField idInputField;
    public TMP_InputField pwInputField;
    public WebSocketManager webLoginManager; // WebLoginManager 인스턴스

    public ModalWindowManager WindowManager;
    public MainPanelManager PanelManager;

    void Update()
    {
        // Tab 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 현재 선택된 인풋 필드에 따라 다음 인풋 필드로 포커스 이동
            if (idInputField.isFocused)
            {
                pwInputField.Select();
            }
            else if (pwInputField.isFocused)
            {
                idInputField.Select();
            }
        }
    }

    public void Login()
    {
        string inputId = idInputField.text;
        string inputPw = pwInputField.text;

        // WebLoginManager를 사용하여 로그인 처리
        webLoginManager.Login(inputId, inputPw);
    }

    public void ShowLoginWarning(string message)
    {
        StartCoroutine(ShowWarningCoroutine(message));
    }

    private IEnumerator ShowWarningCoroutine(string message)
    {
        // 경고창 표시
        //warningPanel.SetActive(true);
        //warningPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        WindowManager.ModalWindowIn();
        // 1.5초 후에 경고창 사라지도록 코루틴 시작
        yield return new WaitForSeconds(1.5f);

        // 경고창 숨기기
        WindowManager.ModalWindowOut();
    }

    public void OnLoginSuccess(string userId)
    {
        Debug.Log("로그인 성공!");
        PanelManager.OpenPanel("Home");
        //WindowManager.ModalWindowIn();
        // 여기에 로그인 성공 후의 작업 추가
        //UIMgr.single.OpenTitle();
        
        PhotonNetwork.NickName = userId;
        GameManager.instance._currentStatus = GameManager.Status._login;
        LoginManager.isLogin = true;
    }
}