using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LoginMgr : MonoBehaviour
{
    public TMP_InputField idInputField;
    public TMP_InputField pwInputField;
    public GameObject warningPanel; // 경고창 GameObject
    

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

        // 입력된 아이디와 비밀번호가 일치하는지 확인
        if (LoginManager.instance.CheckID(inputId))
        {
            if (LoginManager.instance.CheckPwd(inputId, inputPw))
            {
                Debug.Log("로그인 성공!");
                // 여기에 로그인 성공 후의 작업 추가
                UIMgr.single.OpenTitle();    
                GameManager.instance._currentStatus = GameManager.Status._login;
            }
            else
            {
                Debug.Log("로그인 실패: 비밀번호가 올바르지 않습니다.");

                // 경고창 표시
                warningPanel.SetActive(true);
                // 3초 후에 경고창 사라지도록 코루틴 시작
                StartCoroutine(HideWarningAfterDelay());

                // 로그인 실패 시 인풋 필드 초기화
                pwInputField.text = "";
            }
        }
        else
        {
            Debug.Log("로그인 실패: 아이디 또는 비밀번호가 올바르지 않습니다.");

            // 경고창 표시
            warningPanel.SetActive(true);
            // 3초 후에 경고창 사라지도록 코루틴 시작
            StartCoroutine(HideWarningAfterDelay());

            // 로그인 실패 시 인풋 필드 초기화
            idInputField.text = "";
            pwInputField.text = "";
        }
    }

    IEnumerator HideWarningAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // 3초 대기

        // 경고창 숨기기
        warningPanel.SetActive(false);
    }
}
