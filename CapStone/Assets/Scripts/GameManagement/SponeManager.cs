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
        PhotonManager.instance.SetTag("loadScene",true);
        while (!PhotonManager.instance.AllhasTag("loadScene")) yield return null;

        PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[UserData.type].name,
            PhotonManager.instance.spawn_point.position, Quaternion.identity);

        while (!PhotonManager.instance.AllhasTag("loadPlayer")) yield return null;
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
    private void LoadArea()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트가 아닙니다.");
            return;
        }
        LoadingManager.LoadScene("School");
        //SceneLoingsync = SceneManager.LoadSceneAsync("School"); // 스테이지 씬 비동기 로드
    }
    public void Ingame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArea(); // 스테이지 씬 로딩
            //OnStartCreatePlayer("Ingame"); // 플레이어 생성 시작
        }
    }
    private void CreateLobbyList()
    {
        GameObject Player = PhotonNetwork.Instantiate(PhotonManager.instance.list_Prefabs[6].name,Vector3.zero,Quaternion.identity);
        LoadingManager.nextScene = "School";
    }
    private Coroutine _coroutineCreatePlayer;

    private void OnStartCreatePlayer(string Sponetype)
    {
        if (_coroutineCreatePlayer != null)
            StopCoroutine(_coroutineCreatePlayer); // 기존 코루틴 중지

        _coroutineCreatePlayer = StartCoroutine(IEnum_CreatePlayer(Sponetype)); // 새로운 코루틴 시작
    }

    private IEnumerator WaitLoading()
    {
        while (LoadingManager.sceanOp == null || !LoadingManager.sceanOp.isDone)
        {
            yield return null;
        }
    }

    // 플레이어 생성 코루틴
    IEnumerator IEnum_CreatePlayer(string Sponetype)
    {
        yield return StartCoroutine(WaitLoading());
        
        int cnt = 0;
        Debug.Log("코루틴 시작");
        while (LoadingManager.sceanOp.progress < 1f)
        {
            if (cnt > 10000)
            {
                Debug.LogError("스폰 불가");
                yield break; // 스폰 실패 시 종료
            }
            Debug.Log(LoadingManager.sceanOp.progress);
            cnt++;
            yield return null;
        }

        if (Sponetype == "Lobby")CreateLobbyList();
        Debug.Log("생성");
        _coroutineCreatePlayer = null;
    }
}



public class SponeData
{
    public  List<GameObject> list_Prefabs; // 프리팹 리스트
    public  List<Transform> list_ItemSpawnPoints; // 아이템 스폰 포인트 리스트
    public  List<Transform> list_MosterSpawnPoints; // 몬스터 스폰 포인트 리스트
}
