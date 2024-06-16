using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoubleDoorManager : MonoBehaviourPunCallbacks
{
    public bool isOpen= false;
    
    private Animator animator;
    private PhotonView pv;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        pv = this.GetComponent<PhotonView>();
    }
    
    public void ChangeState()
    {
        Debug.Log("실행");
        isOpen = !isOpen;
        pv.RPC("OnChangeStatusDoubleDoor", RpcTarget.AllBuffered, isOpen);
        AnimationUpdate();
    }
    void AnimationUpdate()
    {
        animator.SetBool("isDoubleOpen",isOpen);
    }

    [PunRPC]
    void OnChangeStatusDoubleDoor(bool result)
    {
        isOpen = result;
        if (isOpen)
        {
            Debug.Log("문열림");
        }
        else
        {
            Debug.Log("문닫힘");
        }   
    }
    
}
