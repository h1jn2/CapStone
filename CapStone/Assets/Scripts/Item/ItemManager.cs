using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PhotonView pv;
    private float time = 0f;
    private bool isScaling = false;

    void Start()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (isScaling)
        {
            time += Time.deltaTime;
        }
    }

    [PunRPC]
    public void DestroyItem_RPC()
    {
        Debug.Log("아이템파밍");
        StartCoroutine(DestroyAfterScaling());
    }

    private IEnumerator DestroyAfterScaling()
    {
        yield return StartCoroutine(SetScale(Vector3.one, Vector3.zero, 1f));
        PhotonNetwork.Destroy(this.gameObject);
    }

    private IEnumerator SetScale(Vector3 before, Vector3 after, float settime)
    {
        time = 0f; // 타이머 초기화
        isScaling = true; // 크기 조정 시작

        while (time < settime)
        {
            Vector3 vec = Vector3.Lerp(before, after, time / settime);
            this.gameObject.transform.localScale = vec;
            yield return null;
        }
        // 크기 조정 완료
        this.gameObject.transform.localScale = after;
        isScaling = false;
    }
}
