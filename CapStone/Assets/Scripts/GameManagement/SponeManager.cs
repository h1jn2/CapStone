using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class SponeManager : MonoBehaviour
{
    public static GameObject obj_local; // 로컬 플레이어 객체
    public static Transform spawn_point; // 스폰 포인트

    IEnumerator Start()
    {
        yield return Loading();
    }

    IEnumerator Loading()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        if(LoadingManager.sceanOp.isDone)PhotonManager.instance.SetTag("loadScene",true);
        while (!PhotonManager.instance.AllhasTag("loadScene")) yield return null;

        if(PhotonNetwork.IsMasterClient)
        PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[UserData.type].name,
            PhotonManager.instance.spawn_point.position, Quaternion.identity);
        while (!PhotonManager.instance.MasterGetTag("loadPlayer")) yield return null;
        
        if(!PhotonNetwork.IsMasterClient)PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[UserData.type].name,
            PhotonManager.instance.spawn_point.position, Quaternion.identity);
        
        while (!PhotonManager.instance.AllhasTag("loadPlayer")) yield return null;
        
        ChangeStatus_RPC(GameManager.Status._playing);
        if(PhotonNetwork.IsMasterClient)GameManager.instance.StartGame();
    }
    public static void Spawn_item()
    {
        int[] spawn = new int[5];
        
        for (int i = 0; i < 5; i++)
        {
            spawn[i] = Random.Range(0, 10); // 랜덤 스폰 인덱스 생성
            for (int j = 0; j < i; j++)
            {
                if (spawn[i] == spawn[j])
                {
                    spawn[i] = Random.Range(0, 10); // 중복 방지
                    j = 0;
                }
            }
            Debug.Log(spawn[i]);
        }
            
        for (int i = 0; i < 5; i++)
        {
            int result = spawn[i];
            PhotonNetwork.Instantiate( PhotonManager.instance.list_Prefabs[2].name, PhotonManager.instance.list_ItemSpawnPoints[result].position, Quaternion.identity); // 아이템 프리팹 생성
        }
    }

    // 몬스터 스폰
    public static void Spawn_monster()
    {
        int spawn;
        spawn = Random.Range(0, 4); // 랜덤 스폰 인덱스 생성
        PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[3].name, PhotonManager.instance.list_MosterSpawnPoints[spawn].position, Quaternion.identity); // 몬스터 프리팹 생성
    }
    private void LoadArea()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트가 아닙니다.");
            return;
        }
        LoadingManager.LoadScene("School");
    }
    public void Ingame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArea(); // 스테이지 씬 로딩
        }
    }
    public void ChangeStatus_RPC(GameManager.Status sendStatus)
    {
        GameManager.instance._currentStatus = sendStatus;
        GameManager.instance.PlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        GameManager.instance.AlivePlayerCnt = GameManager.instance.PlayerCnt;
        Debug.Log(sendStatus);
    }

    private IEnumerator WaitLoading()
    {
        while (LoadingManager.sceanOp == null || !LoadingManager.sceanOp.isDone)
        {
            yield return null;
        }
    }
    
}



public class SponeData
{
    public  List<GameObject> list_Prefabs; // 프리팹 리스트
    public  List<Transform> list_ItemSpawnPoints; // 아이템 스폰 포인트 리스트
    public  List<Transform> list_MosterSpawnPoints; // 몬스터 스폰 포인트 리스트
}
