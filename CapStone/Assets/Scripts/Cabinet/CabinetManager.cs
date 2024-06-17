using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
<<<<<<< HEAD
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;
=======
using UnityEngine;
>>>>>>> 296b99d407ab5d591da587765fffcc01d69a6a5b

public class CabinetManager : MonoBehaviourPunCallbacks
{
    public GameObject cabinet;
    public GameObject spawnPoint;
    public GameObject CabinetCamera;
<<<<<<< HEAD

    private CharacterController playerController;
    private PhotonView punview;
    private PlayerControl playerControl;

    private bool isInsideCabinet = false;
=======
    private CharacterController playerController;
    private PhotonView punview;
    private PlayerControl playerControl;
    private Animator playerAnimator;
    private GameObject player;
>>>>>>> 296b99d407ab5d591da587765fffcc01d69a6a5b
    private bool isHiding = false;
    private bool isInsideCabinet = false;
    private static Dictionary<GameObject, bool> cabinetOccupancy = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        punview = GetComponent<PhotonView>();
<<<<<<< HEAD
        playerController = player.GetComponent<CharacterController>();
        playerControl = player.GetComponent<PlayerControl>();
=======
        if (!cabinetOccupancy.ContainsKey(cabinet))
        {
            cabinetOccupancy[cabinet] = false;
        }
    }

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private IEnumerator FindPlayer()
    {
        while (player == null)
        {
            player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                playerController = player.GetComponent<CharacterController>();
                playerControl = player.GetComponent<PlayerControl>();
                playerAnimator = player.GetComponent<Animator>();
            }

            yield return new WaitForSeconds(0.5f);
        }
>>>>>>> 296b99d407ab5d591da587765fffcc01d69a6a5b
    }

    private void Update()
    {
        if (punview.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (IsPlayerNearCabinet() && !cabinetOccupancy[cabinet])
                {
                    punview.RPC("ToggleHideRPC", RpcTarget.AllBuffered);
                }
                else if (isInsideCabinet)
                {
                    punview.RPC("ToggleHideRPC", RpcTarget.AllBuffered);
                }
            }
        }
    }

    private bool IsPlayerNearCabinet()
    {
        float distance = Vector3.Distance(player.transform.position, cabinet.transform.position);
        return distance <= 4f;
    }

    [PunRPC]
    public void ToggleHideRPC()
    {
        if (player == null) return;

        if (isHiding)
        {
            player.transform.position = spawnPoint.transform.position;
            CabinetCamera.SetActive(false);
<<<<<<< HEAD
            isInsideCabinet = false;
            playerControl.enabled = true;
=======
            playerController.enabled = true;
            playerControl.enabled = true;
            playerAnimator.SetBool("isMoving", false);
            playerAnimator.SetBool("isRunning", false);
            cabinetOccupancy[cabinet] = false;
>>>>>>> 296b99d407ab5d591da587765fffcc01d69a6a5b
        }
        else
        {
            player.transform.position = cabinet.transform.position;
            player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            CabinetCamera.SetActive(true);
<<<<<<< HEAD
            isInsideCabinet = true;
            playerControl.enabled = false;
=======
            playerController.enabled = false;
            playerControl.enabled = false;
            playerAnimator.SetBool("isMoving", false);
            playerAnimator.SetBool("isRunning", false);
            cabinetOccupancy[cabinet] = true;
>>>>>>> 296b99d407ab5d591da587765fffcc01d69a6a5b
        }

        isHiding = !isHiding;
        isInsideCabinet = !isInsideCabinet;
    }

    public void ToggleHide()
    {
        punview.RPC("ToggleHideRPC", RpcTarget.AllBuffered);
    }
}
