using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    #endregion

    #region Private Fields
    
    string gameVersion = "0.0.01";
    bool isConnecting;
    
    #endregion

    #region Public Fields
    
        [SerializeField]
        private GameObject controlPanel;
        
        [SerializeField]
        private GameObject progressLabel;

    #endregion

    #region MonoBehaviour CallBacks
    
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    #endregion


    #region Public Methods

    public void Connect()
    {
        isConnecting = true;
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    #endregion
    
    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버연결 성공");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("연결끊김 {0}", cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("현재 생성되어있는 방이 없으므로 새로운 방생성");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom
    });
}

    public override void OnJoinedRoom()
    {
        Debug.Log("방진입완료");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Room for 1' ");

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Room for 1");
        }
    }
    #endregion

}

