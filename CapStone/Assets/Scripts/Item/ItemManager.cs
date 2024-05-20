using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PhotonView pv;
    void Start()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
    }

    public void DestroyItem()
    {
        
    }
}
