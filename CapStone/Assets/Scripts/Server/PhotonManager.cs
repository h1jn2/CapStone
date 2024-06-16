using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    
    /// <summary>
    /// 각종 아이템 및 프리팹들 생성할 위치 
    /// </summary>
    public List<GameObject> list_Prefabs; // 프리팹 리스트
    public List<Transform> list_ItemSpawnPoints; // 아이템 스폰 포인트 리스트
    public List<Transform> list_MosterSpawnPoints; // 몬스터 스폰 포인트 리스트
    public List<Transform> list_PlayerSpawnPoints; // 플레이어 스폰 포인트 리스트
    
    /// <summary>
    /// 게임시작 및 생성에 관련된 객체
    /// </summary>
    
    public UserData LocalDate; // 로컬 유저 데이터
    public Transform spawn_point; // 스폰 포인트
    public GameObject obj_local; // 로컬 플레이어 객체
    public static PhotonManager instance; // 싱글톤 인스턴스
    public bool isLoading; // 로딩 상태
    public static AsyncOperation SceneLoingsync; // 비동기 씬 로드 객체

    /// <summary>
    /// 내부적으로 사용할 변수 및 객체
    /// </summary>
    public List<RoomInfo> RoomInfos;

    public TMP_InputField InputCreateRoomName;
    public TMP_InputField InputJoinRoomName;
    public Image CreateWarning;
    public Image JoinWarning;

    // 게임 실행 중 포톤매니저는 하나만 존재하도록 싱글톤으로 설정
    private void Awake()
    {
        isLoading = false;
        PhotonNetwork.AutomaticallySyncScene = true; // 씬 자동 동기화 설정

        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);    
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject); // 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
            }
        }

        RoomInfos = null;
    }

    // Start 코루틴
    private IEnumerator Start()
    {
        // 로그인 완료까지 대기
        while (LoginManager.isLogin == false)
        {
            yield return null;
        }

        // 프리팹 풀 초기화
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Clear();
        if (pool != null && list_Prefabs != null)
        {
            foreach (var prefab in list_Prefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab); // 프리팹 캐시 추가
            }
        }

        InitPhoton(); // Photon 초기화
    }

    // 서버 설정 초기화
    private void InitPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("서버 연결 전 초기화");
            PhotonNetwork.ConnectUsingSettings(); // 서버 설정을 사용하여 Photon 서버에 연결
        }
    }

    // 방 생성
    private void CreateRoom(string RoomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // 최대 플레이어 수 설정
        PhotonNetwork.CreateRoom(RoomName, roomOptions, TypedLobby.Default); // 방 생성
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        GameManager.instance._currentStatus = GameManager.Status._login;
    }
    
    // 방 입장
    private void JoinRoom(string RoomName)
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4; // 최대 플레이어 수 설정
        PhotonNetwork.JoinOrCreateRoom(RoomName, roomOption, TypedLobby.Default); // 방 입장 또는 생성
    }

    // 스테이지에 플레이어 생성
    private void m_CreatePlayer(UserData m_data)
    {
        if (m_data.gender == 0)
        {
            obj_local = PhotonNetwork.Instantiate(list_Prefabs[0].name, spawn_point.position, Quaternion.identity); // 남성 플레이어 프리팹 생성
        }
        else if (m_data.gender == 1)
        {
            obj_local = PhotonNetwork.Instantiate(list_Prefabs[0].name, spawn_point.position, Quaternion.identity); // 여성 플레이어 프리팹 생성
        }
    }

    // 마스터 클라이언트가 방을 생성 시 스테이지 씬 로딩
    private void LoadArea()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트가 아닙니다.");
            return;
        }
        Debug.Log(PhotonNetwork.LevelLoadingProgress);
        SceneLoingsync = SceneManager.LoadSceneAsync("1.School"); // 스테이지 씬 비동기 로드
    }

    // 아이템 스폰
    private void Spawn_item()
    {
        int[] spawn = new int[5];
        
        for (int i = 0; i < 5; i++)
        {
            spawn[i] = Random.Range(0, 10); // 랜덤 스폰 인덱스 생성
            for (int j = 0; j < i; j++)
            {
                if (spawn[i] == spawn[j])
                {
                    spawn[i] = Random.Range(0, 10); // 중복 방지
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
            PhotonNetwork.Instantiate(list_Prefabs[2].name, list_ItemSpawnPoints[result].position, Quaternion.identity); // 아이템 프리팹 생성
        }
    }

    // 몬스터 스폰
    private void Spawn_monster()
    {
        int spawn;
        spawn = Random.Range(0, 4); // 랜덤 스폰 인덱스 생성
        Debug.LogError("몬스터 스폰위치: " + spawn);
        PhotonNetwork.Instantiate(list_Prefabs[3].name, list_MosterSpawnPoints[spawn].position, Quaternion.identity); // 몬스터 프리팹 생성
    }

    public bool CheckRoomName(string input)
    {
        Debug.Log(RoomInfos.Count);
        bool is_check= false;
        if (RoomInfos.Count == 0)
        {
            is_check = true;
        }
        else
        {
            for (int i = 0; i < RoomInfos.Count; i++)
            {
                if (RoomInfos[i].Name != input)
                {
                    is_check = true;
                }
            }
        }
        return is_check;
    }
    #region ServerCallBacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 마스터 서버 접속 완료");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("포톤 로비 접속 완료");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 룸 리스트 콜백은 로비에 접속했을때 자동으로 호출된다.
        // 로비에서만 호출할 수 있음...
        Debug.Log($"룸 리스트 업데이트 ::::::: 현재 방 갯수 : {roomList.Count}");
        RoomInfos = roomList.ToList();
        Debug.Log($"룸 리스트 업데이트 ::::::: 현재 방 갯수 : {RoomInfos.Count}");
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "RoomState", "Waiting" } }); // 방 상태 설정
            LoadArea(); // 스테이지 씬 로딩
            OnStartCreatePlayer(); // 플레이어 생성 시작
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            m_CreatePlayer(LocalDate); // 로컬 플레이어 생성
        }
        GameManager.instance._currentStatus = GameManager.Status._ready; // 게임 상태 설정
        Debug.Log("현재 인원: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새 인원 진입");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("플레이어 퇴장");
    }

    public override void OnLeftRoom()
    {
        GameManager.instance._currentStatus = GameManager.Status._login; // 게임 상태 설정
        SceneManager.LoadScene("0.MainScene"); // 메인 씬 로드
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }
    
    #endregion

    #region ButtonMethods

    public void btn_click_Mainstart()
    {
        PhotonNetwork.JoinLobby(); // 로비 접속
    }

    public void btn_click_Create()
    {
        string RoomName = InputCreateRoomName.text;
        if (RoomInfos == null)
        {
            Debug.Log("방생성");
            CreateRoom(RoomName);
        }
        else
        {
            if (CheckRoomName(RoomName))
            {
                Debug.Log("방생성");
                CreateRoom(RoomName);
            }
            else
            {
                Debug.Log("방생성불가");
                CreateWarning.gameObject.SetActive(true);
            }
        }
    }

    public void btn_click_Join()
    {
        string RoomName = InputJoinRoomName.text;
        if (RoomInfos == null)
        {
            Debug.Log("방참가불가");
            CreateWarning.gameObject.SetActive(true);
        }
        else
        {
            if (!CheckRoomName(RoomName))
            {
                Debug.Log("방참가");
                JoinRoom(RoomName);
            }
            else
            {
                Debug.Log("방생성불가");
                CreateWarning.gameObject.SetActive(true);
            }
        }
    }
    public void btn_CreateOrJoin(string RoomName)
    {
            JoinRoom(RoomName);
        
    }
    public void btn_click_createRoom()
    {
        CreateRoom("room"); // 방 생성
    }

    public void btn_click_joinroom()
    {
        JoinRoom("room"); // 방 접속 또는 생성
    }

    public void btn_click_StageStart()
    {
        if (PhotonNetwork.IsMasterClient && (GameManager.instance._currentStatus == GameManager.Status._ready))
        {
            Spawn_item(); // 아이템 스폰
            Spawn_monster(); // 몬스터 스폰
        }
    }

    #endregion
    
    #region Coroutines

    private Coroutine _coroutineCreatePlayer;

    private void OnStartCreatePlayer()
    {
        if (_coroutineCreatePlayer != null)
            StopCoroutine(_coroutineCreatePlayer); // 기존 코루틴 중지

        _coroutineCreatePlayer = StartCoroutine(IEnum_CreatePlayer()); // 새로운 코루틴 시작
    }

    // 플레이어 생성 코루틴
    IEnumerator IEnum_CreatePlayer()
    {
        int cnt = 0;
        Debug.Log("코루틴 시작");
        while (SceneLoingsync.progress < 1f)
        {
            if (cnt > 10000)
            {
                Debug.LogError("스폰 불가");
                yield break; // 스폰 실패 시 종료
            }
            Debug.Log(SceneLoingsync.progress);
            cnt++;
            yield return null;
        }

        m_CreatePlayer(LocalDate); // 플레이어 생성
        Debug.Log("생성");
        _coroutineCreatePlayer = null;
    }

    #endregion
}

[System.Serializable]
public class UserData
{
    public int gender; // 성별
    public int type; // 타입
    public string userid; // 유저 아이디

    public UserData()
    {
        gender = 0;
        type = 0;
        userid = "";
    }
}
