using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    public TMP_Text ItemNickname;
    public Image IsAdmin;
    public Button IsKick;
    private LobbyManager LobbyManager;
    public PhotonView pv;
    private string Nickname; 
    IEnumerator Start()
    {
        while (LobbyManager == null)
        {
            LobbyManager = FindObjectOfType<LobbyManager>();
            pv = this.gameObject.GetComponent<PhotonView>();
            Nickname = pv.Owner.NickName;
            yield return null;
        }
        Debug.Log("ui위치설정");
        SetPlayer();
    }

    public void initList()
    {
        for (int i = 0; i < 4; i++)
        {
            if (LobbyManager.Players.Count > 0 && LobbyManager.Players.Count > i)
            {
                if (LobbyManager.Players[i].gameObject == null)
                {
                    Debug.Log("플레이어 삭제요망");
                    LobbyManager.Players.Remove(LobbyManager.Players[i]);
                }    
            }
            
        }
    }
    public void SetPlayer()
    {
        initList();
        ItemNickname.SetText(Nickname);
        if (pv.Owner.IsMasterClient)
        {
            IsAdmin.gameObject.SetActive(true);
        }
        else
        {
            IsAdmin.gameObject.SetActive(false);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if(pv.IsMine)IsKick.gameObject.SetActive(false);
        }
        else
        {
            IsKick.gameObject.SetActive(false);    
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log("i: " + i);
            if (i > (LobbyManager.Players.Count))
            {
                Debug.Log("생성인원초과");
                break;
            }
            if (LobbyManager.PlayerList[i].childCount == 1)
            {   Debug.Log(i);
                this.gameObject.transform.SetParent(LobbyManager.PlayerList[LobbyManager.Players.Count]);    
            }
        }
        this.gameObject.transform.localScale = Vector3.one;
        RectTransform listSize = this.gameObject.GetComponent<RectTransform>();
        listSize.sizeDelta = new Vector2(1000, 150);
        listSize.anchoredPosition = new Vector2(0, 0);
        LobbyManager.Players.Add(this.gameObject);
    }

    void OnDestroy()
    {
        PhotonManager.instance.SetTag("InLobby", true);
        Debug.Log("로비이탈");
    }
}
