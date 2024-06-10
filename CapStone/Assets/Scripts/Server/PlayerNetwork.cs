using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PhotonAnimatorView))]
public class PlayerNetwork : MonoBehaviourPun
{
    public PhotonView pv;
    private int MyPunID;

    private void Awake()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
        MyPunID = this.pv.ViewID;
        Debug.Log(MyPunID);
    }


    public void Check_clear()
    {
        //클리어체크
        
    }
    
    
    public void clicked_stagestart()
    {
        PhotonView photonView = PhotonView.Get(this);
        if (PhotonNetwork.IsMasterClient && GameManager.instance._currentStatus == GameManager.Status._ready)
        {
            photonView.RPC("ChangeStatus_RPC", RpcTarget.All, GameManager.Status._playing);
            Debug.Log("실행");
        }
        
    }
    public void clicked_damege()
    {
        pv.RPC("OnDameged_RPC", RpcTarget.AllBuffered, 100f, pv.ViewID);
    }
    #region Photon RPC
    [PunRPC]
    public void ChangeStatus_RPC(GameManager.Status sendStatus)
    {
        GameManager.instance._currentStatus = sendStatus;
        GameManager.instance.PlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        GameManager.instance.AlivePlayerCnt = GameManager.instance.PlayerCnt;
        Debug.Log(sendStatus);
    }
    //데미지 동기화 함수
    [PunRPC]
    public void OnDameged_RPC(float Damege, int viewID)
    {
        if (pv.ViewID == viewID)
        {
            Debug.Log(pv.ViewID);
            this.GetComponent<PlayerManager>().health -= Damege;
            Debug.Log(this.GetComponent<PlayerManager>().health);
            if (this.GetComponent<PlayerManager>().health <= 0)
            {
                this.GetComponent<PlayerManager>()._isDie = true;
                Debug.Log("플레이어 죽음");
            }
        }
    }
    
    #endregion
}