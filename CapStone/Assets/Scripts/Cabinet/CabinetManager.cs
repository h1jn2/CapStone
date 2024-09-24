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
    private static Dictionary<int, bool> cabinetOccupancy = new Dictionary<int, bool>();


    [SerializeField]
    private AudioSource[] soundPlayer;

    private void Awake()
    {
        punview = GetComponent<PhotonView>();
        int cabinetID = cabinet.GetInstanceID();
        if (!cabinetOccupancy.ContainsKey(cabinetID))
        {
            cabinetOccupancy[cabinetID] = false;
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
                int cabinetID = cabinet.GetInstanceID();
                if (isInsideCabinet)
                {
                    Debug.Log("Attempting to exit cabinet");
                    SoundManager.instance.PlaySound("CloseCabinet", false, soundPlayer);
                    ToggleHide();
                }
                else
                {
                    Debug.Log("Cabinet is already occupied");
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
    public void ToggleHideRPC(int viewID, int cabinetID)
    {
        Debug.Log($"Received RPC: ToggleHideRPC for player {viewID} and cabinet {cabinetID}");

        if (player == null)
        {
            Debug.Log("Player is null");
        }
        else
        {
            Debug.Log($"Player viewID: {player.GetComponent<PhotonView>().ViewID}, Received viewID: {viewID}");
        }

        if (player == null || player.GetComponent<PhotonView>().ViewID != viewID)
        {
            Debug.Log("Player is null or viewID does not match");
            return;
        }

        if (isHiding)
        {
            // Player exiting the cabinet
            Debug.Log("Player exiting the cabinet");
            player.transform.position = spawnPoint.transform.position;
            CabinetCamera.SetActive(false);
            playerController.enabled = true;
            playerControl.enabled = true;
            playerAnimator.SetBool("isMoving", true); // Start moving animation when exiting
            cabinetOccupancy[cabinetID] = false;
        }
        else
        {
            // Player entering the cabinet
            if (cabinetOccupancy[cabinetID])
            {
                Debug.Log("Cabinet is already occupied");
                return;
            }

            Debug.Log("Player entering the cabinet");
            player.transform.position = new Vector3(cabinet.transform.position.x, cabinet.transform.position.y-0.5f, cabinet.transform.position.z);
            player.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            CabinetCamera.SetActive(true);
            playerController.enabled = false;
            playerControl.enabled = false;
            playerAnimator.SetBool("isMoving", false); // Stop moving animation when hiding
            cabinetOccupancy[cabinetID] = true;
        }

        isHiding = !isHiding;
        isInsideCabinet = !isInsideCabinet;
    }

    public void ToggleHide()
    {
        int cabinetID = cabinet.GetInstanceID();
        if (punview.IsMine)
        {
            if (IsPlayerNearCabinet() && !cabinetOccupancy[cabinetID])
            {
                Debug.Log("Attempting to enter cabinet");
                SoundManager.instance.PlaySound("OpenCabinet", false, soundPlayer);
                Hide();
            }
            else if (isInsideCabinet)
            {
                Debug.Log("Attempting to exit cabinet");
                SoundManager.instance.PlaySound("CloseCabinet", false, soundPlayer);
                Hide();
            }
            else
            {
                Debug.Log("Cabinet is already occupied");
            }
        }
        
    }
    private void Hide()
    {
        int cabinetID = cabinet.GetInstanceID();
        if (player != null)
        {
            int playerViewID = player.GetComponent<PhotonView>().ViewID;
            Debug.Log($"Sending RPC ToggleHideRPC for cabinet {cabinetID} and player {playerViewID}");
            punview.RPC("ToggleHideRPC", RpcTarget.AllBuffered, playerViewID, cabinetID);
        }
        else
        {
            Debug.LogError("Player is null in ToggleHide");
        }
    }
}
