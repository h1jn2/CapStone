using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> list_Prefabs;
    public UserData LocalDate;
    //public Transform spawn_point;
    public GameObject obj_local;
    public static PhotonManager instance;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (instance== null)
        {
            instance = this;
            DontDestroyOnLoad(this);    
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Clear();
        if (pool != null && list_Prefabs != null)
        {
            foreach (var prefab in list_Prefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
        InitPhoton();
    }

    private void InitPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("서버연결전 초기화");
            PhotonNetwork.ConnectUsingSettings();
            
        }
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom("test", roomOptions, null);
    }
    private void JoinRoom()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("Capstone", roomOption, TypedLobby.Default);
    }
    private void CreatePlayer(UserData m_data)
    {
        if (m_data.gender == 0)
        {
            obj_local = PhotonNetwork.Instantiate(list_Prefabs[0].name, Vector3.zero, Quaternion.identity);
        }
        else if (m_data.gender == 1)
        {
            obj_local = PhotonNetwork.Instantiate(list_Prefabs[0].name, Vector3.zero, Quaternion.identity);
        }
    }

    private void LoadArea()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트가 아닙니다.");
            return;
        }
        PhotonNetwork.LoadLevel("Test");
    }

    #region ServerCallBacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 마스터 서버 접속완료");
        
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("포톤 로비 접속완료");
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable(){{"RoomState", "Waiting"}});
            LoadArea();
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        CreatePlayer(LocalDate);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArea();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArea();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainScene");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }

    #endregion
    

    #region  buttonMethod

    public void btn_click_start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void btn_click_createRoom()
    {
        CreateRoom();
    }

    #endregion
}

[System.Serializable] 
public class UserData
{
    public int gender;
    public int type;
    public string userid;

    public UserData()
    {
        gender = 0;
        type = 0;
        userid = "";
    }
}
