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

    private void Awake()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
    }
    #region Photon RPC

    public void clicked_stagestart()
    {
        PhotonView photonView = PhotonView.Get(this);
        if (PhotonNetwork.IsMasterClient && GameManager.instance._currentStatus == GameManager.Status._ready && photonView.IsMine)
        {
            photonView.RPC("ChangeStatus_RPC", RpcTarget.All, GameManager.Status._playing);
            Debug.Log("실행");
        }
        
    }
    
    [PunRPC]
    public void ChangeStatus_RPC(GameManager.Status sendStatus)
    {
        GameManager.instance._currentStatus = sendStatus;
        Debug.Log(sendStatus);
    }

    #endregion
    
    //데미지 동기화 함수
    void RpcOnDamaged()
    {
        
    }
}
