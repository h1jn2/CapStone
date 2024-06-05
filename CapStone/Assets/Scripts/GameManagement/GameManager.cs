using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
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
        ItemCnt = 5;
    }
}
