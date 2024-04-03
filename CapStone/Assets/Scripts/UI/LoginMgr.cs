using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LoginMgr : MonoBehaviour
{
    public TMP_InputField idInputField;
    public TMP_InputField pwInputField;
    public GameObject warningPanel; // ���â GameObject

    void Update()
    {
        // Tab Ű�� ���ȴ��� Ȯ��
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // ���� ���õ� ��ǲ �ʵ忡 ���� ���� ��ǲ �ʵ�� ��Ŀ�� �̵�
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

        // �Էµ� ���̵�� ��й�ȣ�� "1234"�� ��ġ�ϴ��� Ȯ��
        if (inputId == "1234" && inputPw == "1234")
        {
            Debug.Log("�α��� ����!");
            // ���⿡ �α��� ���� ���� �۾� �߰�

            UIMgr.single.OpenTitle();
        }
        else
        {
            Debug.Log("�α��� ����: ���̵� �Ǵ� ��й�ȣ�� �ùٸ��� �ʽ��ϴ�.");

            // ���â ǥ��
            warningPanel.SetActive(true);
            // 3�� �Ŀ� ���â ��������� �ڷ�ƾ ����
            StartCoroutine(HideWarningAfterDelay());

            // �α��� ���� �� ��ǲ �ʵ� �ʱ�ȭ
            idInputField.text = "";
            pwInputField.text = "";
        }
    }

    IEnumerator HideWarningAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // 3�� ���

        // ���â �����
        warningPanel.SetActive(false);
    }
}
