using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Dark;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public TMP_InputField InputCreateRoomName;
    public TMP_InputField InputJoinRoomName;
    public ModalWindowManager CreateFail;
    public ModalWindowManager JoinFail;
    
    public void btn_click_Create()
    {
        string RoomName = InputCreateRoomName.text;
        if (PhotonManager.is_CreateWarning)
        {
            //CreateWarning.gameObject.SetActive(false);
            PhotonManager.is_CreateWarning = false;
        }
        else
        {
            if (PhotonManager.instance.RoomInfos == null)
            {
                Debug.Log("방생성");
                PhotonManager.instance.CreateRoom(RoomName);
                PhotonManager.is_CreateWarning = false;
            }
            else
            {
                if (PhotonManager.instance.CheckRoomNameCreate(RoomName))
                {
                    Debug.Log("방생성");
                    PhotonManager.instance.CreateRoom(RoomName);
                    PhotonManager.is_CreateWarning = false;
                }
                else
                {
                    Debug.Log("방생성불가");
                    //CreateWarning.gameObject.SetActive(true);
                    PhotonManager.is_CreateWarning = true;
                }
            }
        }
        
    }
    public void btn_click_Join()
    {
        string RoomName = InputJoinRoomName.text;
        if (PhotonManager.is_JoinWarning)
        {
            //JoinWarning.gameObject.SetActive(false);
            PhotonManager.is_JoinWarning = false;
        }
        else
        {
            if (PhotonManager.instance.RoomInfos == null)
            {
                Debug.Log("방참가불가");
                //CreateWarning.gameObject.SetActive(true);
                PhotonManager.is_JoinWarning = true;
            }
            else
            {
                if (PhotonManager.instance.CheckRoomNameJoin(RoomName))
                {
                    Debug.Log("방참가");
                    PhotonManager.instance.JoinRoom(RoomName);
                }
                else
                {
                    Debug.Log("방생성불가");
                    //JoinWarning.gameObject.SetActive(true);
                    PhotonManager.is_JoinWarning = true;
                }
            }    
        }
        
    }
}
