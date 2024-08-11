using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoorManager : MonoBehaviourPunCallbacks
{
    public bool isOpen= false;
    
    private Animator animator;
    private PhotonView pv;

    [SerializeField]
    private AudioSource[] soundPlayer;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        pv = this.GetComponent<PhotonView>();
    }
    
    public void ChangeState()
    {
        Debug.Log("실행");

        if (!isOpen)
            SoundManager.instance.PlaySound("OpenDoor", false, soundPlayer);
        else
            SoundManager.instance.PlaySound("CloseDoor", false, soundPlayer);

        isOpen = !isOpen;
        pv.RPC("OnChangeStatus", RpcTarget.AllBuffered, isOpen);
        

        AnimationUpdate();
    }
    void AnimationUpdate()
    { 
        animator.SetBool("isOpen", isOpen); 
    }

    [PunRPC]
    void OnChangeStatus(bool result)
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
