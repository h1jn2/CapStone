using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //public GameClear1 clearManager;
    public enum Status
    {
        _none,
        _login,
        _ready,
        _exit,
        _end,
        _playing
    }
    
    public Status _currentStatus;
    public int PlayerCnt;
    public int ItemCnt;
    public int AlivePlayerCnt;

    public static event Action OnStartGame;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitGame();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitGame()
    {
        _currentStatus = Status._none;
        PlayerCnt = 0;
        AlivePlayerCnt = 0;
        ItemCnt = 5;
        OnStartGame += SponeManager.Spawn_item;
        OnStartGame += SponeManager.Spawn_monster;
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void check_clear()
    {
        Debug.Log("log");
        if (GameManager.instance._currentStatus == Status._playing)
        {
            OnStartGame -= SponeManager.Spawn_item;
            OnStartGame -= SponeManager.Spawn_monster;
            if (GameManager.instance.AlivePlayerCnt >= 0 && GameManager.instance.ItemCnt == 0)
            {
                GameManager.instance._currentStatus = Status._end;
                GameClear1.instance.OnGameClear();
                
                //클리어화면 active
                //나올 내용 설정
            }
            else if (GameManager.instance.AlivePlayerCnt <= 0)
            {
                GameManager.instance._currentStatus = Status._end;
                GameClear1.instance.OnGameLose();
                //PhotonManager.instance.LeaveRoom();//임시 테스트 코드
            }
        }
    }
}

[System.Serializable]
public static class UserData
{
    public static int gender; // 성별
    public static int type; // 타입
    public static string userid; // 유저 아이디
    
}