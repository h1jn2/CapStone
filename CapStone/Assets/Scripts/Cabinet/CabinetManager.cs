using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CabinetManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cabinet;
    public GameObject spawnPoint;

    private CharacterController playerController;
    private PhotonView punview;

    private bool isInsideCabinet = false;
    private bool isHiding = false;

    private void Awake()
    {
        punview = GetComponent<PhotonView>();
        playerController = player.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (punview.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (IsPlayerNearCabinet())
                {
                    ToggleHide();
                }
                else if (isInsideCabinet)
                {
                    ToggleHide();
                }
            }

            if (isInsideCabinet)
            {
                playerController.enabled = false;
            }
            else
            {
                playerController.enabled = true;
            }
        }
    }

    private bool IsPlayerNearCabinet()
    {
        float distance = Vector3.Distance(transform.position, cabinet.transform.position);
        return distance <= 7f;
    }

    private void ToggleHide()
    {
        if (isHiding)
        {
            player.transform.position = spawnPoint.transform.position;
            isInsideCabinet = false;
        }
        else
        {
            player.transform.position = cabinet.transform.position;
            isInsideCabinet = true;
        }

        isHiding = !isHiding;
    }
}
