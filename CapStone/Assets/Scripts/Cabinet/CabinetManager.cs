using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CabinetManager : MonoBehaviourPunCallbacks
{
    public GameObject cabinet;
    public GameObject spawnPoint;
    public GameObject CabinetCamera;
    private CharacterController playerController;
    private PhotonView punview;
    private PlayerControl playerControl;
    private Animator playerAnimator;
    private GameObject player;
    private bool isHiding = false;
    private bool isInsideCabinet = false;
    private static Dictionary<GameObject, bool> cabinetOccupancy = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        punview = GetComponent<PhotonView>();
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
            playerController.enabled = true;
            playerControl.enabled = true;
            playerAnimator.SetBool("isMoving", false);
            playerAnimator.SetBool("isRunning", false);
            cabinetOccupancy[cabinet] = false;
        }
        else
        {
            player.transform.position = cabinet.transform.position;
            player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            CabinetCamera.SetActive(true);
            playerController.enabled = false;
            playerControl.enabled = false;
            playerAnimator.SetBool("isMoving", false);
            playerAnimator.SetBool("isRunning", false);
            cabinetOccupancy[cabinet] = true;
        }

        isHiding = !isHiding;
        isInsideCabinet = !isInsideCabinet;
    }

    public void ToggleHide()
    {
        punview.RPC("ToggleHideRPC", RpcTarget.AllBuffered);
    }
}
