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
    public static PhotonView ppv;

    private void Awake()
    {
        ppv = this.gameObject.GetComponent<PhotonView>();
    }

    //데미지 동기화 함수
    void RpcOnDamaged()
    {
        
    }
}
