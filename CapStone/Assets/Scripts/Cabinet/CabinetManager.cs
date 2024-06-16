using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class CabinetManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cabinet;
    public GameObject spawnPoint;
    public GameObject CabinetCamera;

    private CharacterController playerController;
    private PhotonView punview;
    private PlayerControl playerControl;

    private bool isInsideCabinet = false;
    private bool isHiding = false;

    private void Awake()
    {
        punview = GetComponent<PhotonView>();
        playerController = player.GetComponent<CharacterController>();
        playerControl = player.GetComponent<PlayerControl>();
    }

    private void Update()
    {
        if (punview.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
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
        float distance = Vector3.Distance(player.transform.position, cabinet.transform.position);
        return distance <= 4f;
    }

    private void ToggleHide()
    {
        if (isHiding)
        {
            player.transform.position = spawnPoint.transform.position;
            CabinetCamera.SetActive(false);
            isInsideCabinet = false;
            playerControl.enabled = true;
        }
        else
        {
            player.transform.position = cabinet.transform.position;
            player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            CabinetCamera.SetActive(true);
            isInsideCabinet = true;
            playerControl.enabled = false;
        }

        isHiding = !isHiding;
    }
}
