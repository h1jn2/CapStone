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
        pv.RPC("OnEquieItem_RPC", RpcTarget.All);
        Debug.Log("아이템파밍");
        SetScale(Vector3.one, Vector3.zero, 1f);
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    public void OnEquieItem_RPC()
    {
        GameManager.instance.ItemCnt--;
    }
    
    IEnumerator SetScale(Vector3 before, Vector3 after, float settime)
    {
        float timer = 0;
        while (timer < settime)
        {
            timer += Time.deltaTime;
            Vector3 vec = Vector3.Lerp(before, after, timer / settime);
            this.gameObject.transform.localScale = vec;
            yield return null;
        }
    }
}
