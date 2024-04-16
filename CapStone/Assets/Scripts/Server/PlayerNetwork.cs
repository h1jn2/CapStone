using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PhotonAnimatorView))]
public class PlayerNetwork : MonoBehaviourPun
{

    //데미지 동기화 함수
    void RpcOnDamaged()
    {
        
    }
}
