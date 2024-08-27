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
        pm = this.gameObject.GetComponent<PlayerManager>();
        pv = this.gameObject.GetComponent<PhotonView>();
        if (!pv.IsMine)
            playerCamera.SetActive(false);

        GameManager.instance.PlayerCameras.Add(playerCamera);
        playerindex = 0;
    }

    private void Update()
    {
        if (pm._isDie && pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {   
                Debug.Log("관전");
                Debug.Log(playerindex);
                Debug.Log(PhotonNetwork.CountOfPlayers);
                GameManager.instance.PlayerCameras[playerindex].gameObject.SetActive(false);
                if ((playerindex+1) == PhotonNetwork.CountOfPlayers) playerindex = 0;
                else playerindex++;
                GameManager.instance.PlayerCameras[playerindex].gameObject.SetActive(true);
            }
        }

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
