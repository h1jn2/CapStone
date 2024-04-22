using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoorManager : MonoBehaviour
{
    public bool isOpen= false;
    
    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        AnimationUpdate();
    }

    public void ChangeState()
    {
        Debug.Log("실행");
        isOpen = !isOpen;
    }
    void AnimationUpdate()
    {
        animator.SetBool("isOpen", isOpen);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("문접근");
    }
    */
}
