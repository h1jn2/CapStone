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

    [PunRPC]
    public void DestroyItem_RPC()
    {
        Debug.Log("아이템파밍");
        GameManager.instance.ItemCnt--;
        ItemUi.instance.UpdateCount();
        GameManager.instance.check_clear();
        if (pv.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
