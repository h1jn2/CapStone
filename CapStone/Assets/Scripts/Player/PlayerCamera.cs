using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private PhotonView pv;
    public PlayerManager pm;
    public GameObject playerCamera;
    private int playerindex;

    void Start()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
        if (!pv.IsMine)
            playerCamera.SetActive(false);

        GameManager.instance.PlayerCameras.Add(playerCamera);
        Debug.Log(GameManager.instance.PlayerCameras);
        playerindex = 0;
    }

    private void Update()
    {
        

    }

    public void ChangeCamera()
    {
        bool isChange=false;

        while (!isChange)
        {
            if (playerindex > PhotonNetwork.CountOfPlayers) playerindex = 0;
            if ((PhotonNetwork.PlayerList[playerindex] == PhotonNetwork.LocalPlayer)) playerindex++;
            else
            {
                if (!PhotonNetwork.PlayerList[playerindex].CustomProperties["isDie"].Equals(false))
                {
                    playerCamera.gameObject.SetActive(false);
                    GameManager.instance.PlayerCameras[playerindex].gameObject.SetActive(true);
                    isChange = true;
                }
                else
                {
                    playerindex++;
                }
                
            }
        }
    }

}
