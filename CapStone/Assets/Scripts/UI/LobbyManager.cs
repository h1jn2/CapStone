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
        Debug.Log(PhotonNetwork.InRoom);
    }

    public void ExitRoom()
    {
        PhotonManager.instance.DisconnectRoom();
    }
}
