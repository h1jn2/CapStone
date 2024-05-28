using System.Collections;
using System.Collections.Generic;
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

        _currentStatus = Status._none;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
