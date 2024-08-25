using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public List<Transform> PlayerList;
    public TMP_Text RoomName;
    public List<GameObject> Players;  
    
    private void Start()
    {
        RoomName.SetText(PhotonNetwork.CurrentRoom.Name);
    }

    public void ExitRoom()
    {
        PhotonManager.instance.DisconnectRoom();
    }
    public void GameStrat()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadingManager.sceanOp = null;
            StartCoroutine(DestroyLobby());
        }
    }

    IEnumerator DestroyLobby()
    {
        PhotonNetwork.DestroyAll();
        while (!PhotonManager.instance.AllhasTag("InLobby")) yield return null;
        PhotonManager.instance.Ingame();
        PhotonNetwork.IsMessageQueueRunning = false;
    }
        
}
