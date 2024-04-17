using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public float health = 100f;

    public static GameObject LocalPlayerInstance;

    private void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            this.gameObject.name += "(LocalPlayer)";
        }
        else
        {
            this.gameObject.name += "(OtherPlayer)";
        }
    }
}
