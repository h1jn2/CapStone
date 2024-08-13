using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using System;
using System.Net;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public float health = 100f;
    public bool _isDie;

    public static GameObject LocalPlayerInstance;
    public GameObject playerCamera;

    public event Action OnPlayerdied;
    public event Action OnPlayerRespone;

    private void Awake()
    {
        PhotonManager.instance.SetTag("loadPlayer",true);
        OnPlayerdied += OnDemeged;
        OnPlayerRespone += OnRespone;
    }

    private void OnDestroy()
    {
       OnPlayerdied -= OnDemeged;
       OnPlayerRespone -= OnRespone;
    }

    private void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            this.gameObject.name += "(LocalPlayer)";
            
        }
        else
        {
            this.gameObject.name += "(OtherPlayer)";
            ChangeLayerRecursively(this.gameObject,0);
            playerCamera.SetActive(false);
        }
    }   

    private void ChangeLayerRecursively(GameObject obj, int layer)
    {
        Debug.Log("camera_set");
        int numOfChild = obj.transform.childCount;
        for (int i = 0; i < numOfChild; i++)
        {
            transform.GetChild(i).gameObject.layer = layer;

        }
    }

    public void OnDemeged()
    {
        this._isDie = true;
        GameManager.instance.AlivePlayerCnt--; //공격시 생존인원  변수 감소
        GameManager.instance.check_clear();
        this.GetComponent<CharacterController>().enabled = false; // 플레이어 맵에 존재하면 순찰 경로로 변경이 안 돼서 일단 이렇게 해놔씀
    }

    public void OnRespone()
    {
        this._isDie = false;
        GameManager.instance.AlivePlayerCnt++; //공격시 생존인원  변수 감소
        GameManager.instance.check_clear();
        this.GetComponent<CharacterController>().enabled = true; // 플레이어 맵에 존재하면 순찰 경로로 변경이 안 돼서 일단 이렇게 해놔씀
    }
    public void Die()
    {
        OnPlayerdied?.Invoke();
    }
}
