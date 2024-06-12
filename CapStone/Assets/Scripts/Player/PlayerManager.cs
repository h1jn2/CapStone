using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public float health = 100f;
    public bool _isDie;

    public static GameObject LocalPlayerInstance;
    public GameObject playerCamera;

    private void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            this.gameObject.name += "(LocalPlayer)";
            
        }
        else
        {
            this.gameObject.name += "(OtherPlayer)";
            ChangeLayerRecursively(this.gameObject,0);
            playerCamera.SetActive(false);
        }
    }   

    private void ChangeLayerRecursively(GameObject obj, int layer)
    {
        Debug.Log("camera_set");
        int numOfChild = obj.transform.childCount;
        for (int i = 0; i < numOfChild; i++)
        {
            transform.GetChild(i).gameObject.layer = layer;

        }
    }
}
