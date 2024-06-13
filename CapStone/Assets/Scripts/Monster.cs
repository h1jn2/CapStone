using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviourPun
{
    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;
    public Collider short_enemy;

    private NavMeshAgent nvAgent;

    private Vector3[] wayPoints;
    private Vector3 targetWayPoint;
    private float arrivalDist = 1.5f;
    public float navDistance;
    public float waitTime = 3.0f;

    private IEnumerator patrolCoroutine;
    private IEnumerator chaseCoroutine;
    private IEnumerator attackCoroutine;

    private PhotonView monsterPv;



    private enum State
    {
        Patrol,
        Chase,
        Attack,
        Idle
    }

    [SerializeField]
    private State _curState;

    private void Awake()
    {
        monsterPv = this.gameObject.GetPhotonView();
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        wayPoints = new[] { new Vector3(-30, this.transform.position.y, 110), new Vector3(-30, this.transform.position.y, 130) };
    }

    private void Start()
    {
        _curState = State.Patrol;

        patrolCoroutine = StartPatrol();
        chaseCoroutine = StartChase();
        attackCoroutine = StartAttack();
    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);


        if (colliders.Length != 0)
        {
            short_enemy = colliders[0];
        }
        
        Debug.Log(_curState);
        switch (_curState)
        {
            case State.Patrol:
                StartCoroutine(patrolCoroutine);

                if (CanSeePlayer())
                {
                    StopCoroutine(patrolCoroutine);
                    _curState = State.Chase;
                    break;
                }
                if (GameManager.instance.AlivePlayerCnt == 0)
                {
                    _curState = State.Idle;
                    break;
                }
                break;
            case State.Chase:
                StartCoroutine(chaseCoroutine);

                if (!CanSeePlayer())
                {
                    StopCoroutine(chaseCoroutine);
                    _curState = State.Patrol;
                    break;
                }
                if (navDistance < arrivalDist)
                {
                    StopCoroutine(chaseCoroutine);
                    _curState = State.Attack;
                    break;
                }
                break;
            case State.Attack:
                StartCoroutine(attackCoroutine);

                if (!CanSeePlayer())
                {
                    StopCoroutine(attackCoroutine);
                    _curState = State.Patrol;
                    break;
                }
                break;
            case State.Idle:
                StopAllCoroutines();
                break;
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1))
        {
            if (hit.collider == null)
                return;
            if (hit.collider.CompareTag("Door"))
            {
                if (!hit.collider.GetComponent<DoorManager>().isOpen)
                    hit.collider.GetComponent<DoorManager>().ChangeState();
            }
        }


    }

    private bool CanSeePlayer()
    {
        if (colliders.Length > 0 && short_enemy != null)
        {
            if (!short_enemy.GetComponent<PlayerManager>()._isDie)
            {
                return true;
            }
            short_enemy = null;
            return false;
        }
        else
        {
            short_enemy = null;
            return false;
        }
    }

    private IEnumerator StartPatrol()
    {
        Debug.Log("순찰 중");

        while (true)
        {
            if (navDistance < arrivalDist)
            {
                targetWayPoint = wayPoints[Random.Range(0, 2)];
                yield return null;
            }

            nvAgent.SetDestination(targetWayPoint);
            navDistance = Vector3.Distance(this.transform.position, targetWayPoint);
            yield return StartCoroutine(patrolCoroutine);
        }
    }

    private IEnumerator StartChase()
    {
        Debug.Log("플레이어 추격");

        while (true)
        {
            if (colliders.Length != 0)
            {
                float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
                short_enemy = colliders[0];
                foreach (Collider col in colliders)
                {
                    float short_distance2 = Vector3.Distance(transform.position, col.transform.position);

                    if (short_distance > short_distance2)
                    {
                        short_distance = short_distance2;
                        short_enemy = col;
                    }
                }

                nvAgent.SetDestination(short_enemy.transform.position);
                navDistance = Vector3.Distance(this.transform.position, short_enemy.transform.position);
                //Debug.Log(short_enemy + ", " + navDistance);

            }
            yield return null;
        }
    }

    private IEnumerator StartAttack()
    {
        Debug.Log("공격 중");
        short_enemy.GetComponent<PlayerManager>()._isDie = true;
        GameManager.instance.AlivePlayerCnt--; //공격시 생존인원  변수 감소
        //monsterPv.RPC("OnDemegePlayer_RPC",RpcTarget.All);
        GameManager.instance.check_clear();
        //GameObject.Find("GameManager").GetComponent<GameManager>()._currentStatus = GameManager.Status._end; //=> 현재 한명이라도 공격당할시 모든 클라이언트가 정지함
        navDistance = Vector3.Distance(this.transform.position, short_enemy.transform.position);
        short_enemy.gameObject.GetComponent<CharacterController>().enabled = false;       // 플레이어 맵에 존재하면 순찰 경로로 변경이 안 돼서 일단 이렇게 해놔씀
        short_enemy = null;
        yield return null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
    ////////////////////////////
    //게임클리어조건을 위해 게임매니저 함수 동기화 RPC => 공격시 생존인원 카운트
    ////////////////////////////

    [PunRPC]
    public void OnDemegePlayer_RPC()
    {
        if (monsterPv.IsMine)
        {
            Debug.Log("데미지동기화");
            GameManager.instance.AlivePlayerCnt--;
        }
            
    }
}
