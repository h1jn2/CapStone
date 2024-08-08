using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SponeManager : MonoBehaviour
{
    public static GameObject obj_local; // 로컬 플레이어 객체
    public static Transform spawn_point; // 스폰 포인트
    
    public static void m_CreatePlayer()
    {
        if (UserData.gender == 0)
        {
            PhotonManager.instance.obj_local  = PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[UserData.type].name, spawn_point.position, Quaternion.identity); // 남성 플레이어 프리팹 생성
        }
        else if (UserData.gender == 1)
        {
            PhotonManager.instance.obj_local = PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[UserData.type].name, spawn_point.position, Quaternion.identity); // 여성 플레이어 프리팹 생성
        }
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
                    Debug.LogWarning(spawn[i]);
                    j = 0;
                }
            }
            Debug.Log(spawn[i]);
        }
            
        for (int i = 0; i < 5; i++)
        {
            int result = spawn[i];
            Debug.LogError(result);
            PhotonNetwork.Instantiate( PhotonManager.instance.list_Prefabs[2].name, PhotonManager.instance.list_ItemSpawnPoints[result].position, Quaternion.identity); // 아이템 프리팹 생성
        }
    }

    // 몬스터 스폰
    public static void Spawn_monster()
    {
        int spawn;
        spawn = Random.Range(0, 4); // 랜덤 스폰 인덱스 생성
        Debug.LogError("몬스터 스폰위치: " + spawn);
        PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[3].name, PhotonManager.instance.list_MosterSpawnPoints[spawn].position, Quaternion.identity); // 몬스터 프리팹 생성
    }
}

public class SponeData
{
    public  List<GameObject> list_Prefabs; // 프리팹 리스트
    public  List<Transform> list_ItemSpawnPoints; // 아이템 스폰 포인트 리스트
    public  List<Transform> list_MosterSpawnPoints; // 몬스터 스폰 포인트 리스트
}
