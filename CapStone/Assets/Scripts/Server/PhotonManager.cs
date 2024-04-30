using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;



public class PhotonManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> list_Prefabs;
    public List<Transform> list_ItemSpawnPoints;
    public List<Transform> list_MosterSpawnPoints;
    public List<Transform> list_PlayerSpawnPoints;
    
    public UserData LocalDate;
    public Transform spawn_point;
    public GameObject obj_local;
    public static PhotonManager instance;
    public bool isLoading;
    public static AsyncOperation SceneLoingsync;


    //게임을 실행중 포톤매니저는 무조건 하나만 있어야되기때문에 싱클톤으로 실행
    private void Awake()
    {
        isLoading = false;
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

    //서버설정초기화
    private void InitPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("서버연결전 초기화");
            PhotonNetwork.ConnectUsingSettings();
            
        }
    }

    //방생성
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom("Capstone", roomOptions, TypedLobby.Default);
    }
    
    //방입장
    private void JoinRoom()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("Capstone", roomOption, TypedLobby.Default);
    }

    //스테이지에 플레이어 생성
    private void m_CreatePlayer(UserData m_data)
    {
        if (m_data.gender == 0)
        {
            obj_local = PhotonNetwork.Instantiate(list_Prefabs[0].name, spawn_point.position, Quaternion.identity);
        }
        else if (m_data.gender == 1)
        {
            obj_local = PhotonNetwork.Instantiate(list_Prefabs[0].name, spawn_point.position, Quaternion.identity);
        }
    }
    //마스터 클라이언트가 방을 생성시 스테이지씬 로딩
    private void LoadArea()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트가 아닙니다.");
            return;
        }
        Debug.Log(PhotonNetwork.LevelLoadingProgress);
        //PhotonNetwork.LoadLevel("School");
        SceneLoingsync = SceneManager.LoadSceneAsync("School");
    }

    private void Spawn_item()
    {
        int[] spawn = new int [5];
        if(true)
        {
            for (int i = 0; i < 5; i++)
            {
                spawn[i] = Random.Range(0, 10);
                for (int j = 0; j < i; j++)
                {
                    if (spawn[i] == spawn[j])
                    {
                        spawn[i] = Random.Range(0, 10);
                        Debug.LogWarning(spawn[i]);
                        j = 0;
                    }
                }
                Debug.Log(spawn[i]);
            }
            
            for (int i = 0; i < 5; i++)
            {
                int result = spawn[i];
                Debug.LogError(result);
                PhotonNetwork.Instantiate(list_Prefabs[2].name, list_ItemSpawnPoints[result].position, Quaternion.identity);
            }
        }
    }

    private void Spawn_monster()
    {
        int spawn;

        if (true)
        {
            spawn = Random.Range(0, 4);
            Debug.LogError("몬스터 스폰위치: " + spawn);
            PhotonNetwork.Instantiate(list_Prefabs[3].name, list_MosterSpawnPoints[spawn].position, Quaternion.identity);
        }
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
            OnStartCreatePlayer();
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            m_CreatePlayer(LocalDate);
        }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새인원 진입");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("플레이어 퇴장");
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

    public void btn_click_Mainstart()
    {
        PhotonNetwork.JoinLobby();
    }

    public void btn_click_createRoom()
    {
        CreateRoom();
    }

    public void btn_click_joinroom()
    {
        JoinRoom();
    }

    public void btn_click_StageStart()
    {
        
        Spawn_item();
        Spawn_monster();
    }

    #endregion
    
    #region 코루틴

    private Coroutine _coroutineCreatePlayer;
    private void OnStartCreatePlayer()
    {
        if(_coroutineCreatePlayer != null)
            StopCoroutine(_coroutineCreatePlayer);

        _coroutineCreatePlayer = StartCoroutine(IEnum_CreatePlayer());
    }

    
    IEnumerator IEnum_CreatePlayer()
    {
            
        int cnt=0;
        Debug.Log("코루틴 시작");
        while (SceneLoingsync.progress < 1f)
        {
            if (cnt > 10000)
            {
                Debug.LogError("스폰불가");
                yield break;
            }
            Debug.Log(SceneLoingsync.progress);
            cnt++;
            yield return null;
        }
        
        m_CreatePlayer(LocalDate);
        Debug.Log("생성");
        _coroutineCreatePlayer = null;
        yield break;
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
