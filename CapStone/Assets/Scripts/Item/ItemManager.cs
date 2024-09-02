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
        transform.Rotate(new Vector3(20, 0, 0));
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
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
