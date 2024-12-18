using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    //public List<Transform> list_PlayerSpawnPoints; // 플레이어 스폰 포인트 리스트
    public int Spawntype;
    
    /// <summary>
    /// 게임시작 및 생성에 관련된 객체
    /// </summary>
    
    //public UserData LocalDate; // 로컬 유저 데이터
    public Transform spawn_point; // 스폰 포인트
    public GameObject obj_local; // 로컬 플레이어 객체
    public static PhotonManager instance; // 싱글톤 인스턴스
    public bool isLoading; // 로딩 상태
    //public static AsyncOperation SceneLoingsync; // 비동기 씬 로드 객체

    /// <summary>
    /// 내부적으로 사용할 변수 및 객체
    /// </summary>
    public List<RoomInfo> RoomInfos;

    
    public static bool is_CreateWarning;
    public static bool is_JoinWarning;

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
        Spawntype = 0;
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
    public void CreateRoom(string RoomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // 최대 플레이어 수 설정
        PhotonNetwork.CreateRoom(RoomName, roomOptions, TypedLobby.Default); // 방 생성
    }
    // 방 입장
    public void JoinRoom(string RoomName)
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4; // 최대 플레이어 수 설정
        PhotonNetwork.JoinOrCreateRoom(RoomName, roomOption, TypedLobby.Default); // 방 입장 또는 생성
    }
    private void CreateLobbyList()
    {
        GameObject Player = PhotonNetwork.Instantiate(list_Prefabs[6].name,Vector3.zero,Quaternion.identity);
        LoadingManager.nextScene = "School";
    }
    

    // 마스터 클라이언트가 방을 생성 시 스테이지 씬 로딩
    private void LoadArea()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트가 아닙니다.");
            return;
        }
        LoadingManager.LoadScene("School");
        //SceneLoingsync = SceneManager.LoadSceneAsync("School"); // 스테이지 씬 비동기 로드
    }

    private void JoinLobby()
    {
        LoadingManager.sceanOp = null;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        LoadingManager.LoadScene("Lobby 1");
    }

    public bool CheckRoomNameCreate(string input)
    {
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
    public bool CheckRoomNameJoin(string input)
    {
        Debug.Log(RoomInfos.Count);
        bool is_check= false;
        if (RoomInfos.Count == 0)
        {
            is_check = false;
        }
        else
        {
            for (int i = 0; i < RoomInfos.Count; i++)
            {
                if (RoomInfos[i].Name == input)
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
            Debug.Log("방생성중 플레이어생성시작");
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "RoomState", "Waiting" } }); // 방 상태 설정
            JoinLobby();
            OnStartCreatePlayer("Lobby");
            //CreateLobbyList();
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            CreateLobbyList();
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
        Debug.Log("로비나감");
    }

    #endregion

    #region ButtonMethods

    public void btn_click_Mainstart()
    {
        PhotonNetwork.JoinLobby(); // 로비 접속
    }
    public void btn_CreateOrJoin(string RoomName)
    {
            JoinRoom(RoomName);
    }
    public void btn_woman1()
    {
        UserData.type = 0;
    }
    public void btn_woman2()
    {
        UserData.type = 1;
    }
    public void btn_man1()
    {
        UserData.type = 4;
    }
    public void btn_man2()
    {
        UserData.type = 5;
    }
    

    private Coroutine _coroutineDisconnectRoom = null;

    public void DisconnectRoom()
    {
        if (_coroutineDisconnectRoom != null)
            return;
        _coroutineDisconnectRoom = StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        if (!PhotonNetwork.LeaveRoom())
        {
            Debug.LogError("Fail LeaveRom!");
            yield break;
        }
            
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        
        Debug.Log("방 나감");
        
        while (!PhotonNetwork.InLobby)
        {
            Debug.Log(PhotonNetwork.NetworkingClient.State);
            yield return null;
        }
        Debug.Log("로비 입장");
        
        _coroutineDisconnectRoom = null;
        GameManager.instance._currentStatus = GameManager.Status._login;
        LoadingManager.LoadScene("0.MainScene");
    }

    public void Ingame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArea(); // 스테이지 씬 로딩
            //OnStartCreatePlayer("Ingame"); // 플레이어 생성 시작
        }
    }
    
    
    #endregion

    #region PlayerTag

    public void SetTag(string key, object value, Player player = null)
    {
        if (player == null) player = PhotonNetwork.LocalPlayer;
        player.SetCustomProperties(new Hashtable{{key, value}});
    }

    public object GetTag(Player player, string key)
    {
        if (player == null) player = PhotonNetwork.LocalPlayer;
        return player.CustomProperties[key].ToString();
    }

    public bool MasterGetTag(string key)
    {
        if (PhotonNetwork.MasterClient.CustomProperties[key] == null) return false;
        else return true;
    }

    public bool AllhasTag(string key)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].CustomProperties[key] == null) return false;
        }
        return true;
    }
    

    #endregion
    
    #region Coroutines

    private Coroutine _coroutineCreatePlayer;

    private void OnStartCreatePlayer(string Sponetype)
    {
        if (_coroutineCreatePlayer != null)
            StopCoroutine(_coroutineCreatePlayer); // 기존 코루틴 중지

        _coroutineCreatePlayer = StartCoroutine(IEnum_CreatePlayer(Sponetype)); // 새로운 코루틴 시작
    }

    private IEnumerator WaitLoading()
    {
        while (LoadingManager.sceanOp == null || !LoadingManager.sceanOp.isDone)
        {
            yield return null;
        }
    }

    // 플레이어 생성 코루틴
    IEnumerator IEnum_CreatePlayer(string Sponetype)
    {
        yield return StartCoroutine(WaitLoading());
        
        int cnt = 0;
        Debug.Log("코루틴 시작");
        while (LoadingManager.sceanOp.progress < 1f)
        {
            if (cnt > 10000)
            {
                Debug.LogError("스폰 불가");
                yield break; // 스폰 실패 시 종료
            }
            Debug.Log(LoadingManager.sceanOp.progress);
            cnt++;
            yield return null;
        }

        if (Sponetype == "Lobby")CreateLobbyList();
        Debug.Log("생성");
        _coroutineCreatePlayer = null;
    }

    #endregion
}


