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

    private Coroutine currentCoroutine;

    private PhotonView monsterPv;

    private IEnumerator patrolCoroutine;
    private IEnumerator chaseCoroutine;
    private IEnumerator attackCoroutine;

    [SerializeField]
    private AudioSource[] soundPlayer;

    [SerializeField]
    private Animator anim;

    private float time;

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
        wayPoints = new[] { new Vector3(70, this.transform.position.y, 115), new Vector3(19, this.transform.position.y, 105)
        , new Vector3(19, this.transform.position.y, 100), new Vector3(47, this.transform.position.y, 96)};
        targetWayPoint = wayPoints[0];
    }

    private void Start()
    {
        patrolCoroutine = StartPatrol();
        chaseCoroutine = StartChase();
        attackCoroutine = StartAttack();
        ChangeState(State.Patrol);
    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0 )
        {
            float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            short_enemy = colliders[0];

            foreach (Collider col in colliders)
            {
                if (col.GetComponent<PlayerManager>()._isDie)
                    continue;
                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);

                if (short_distance > short_distance2)
                {
                    short_distance = short_distance2;
                    short_enemy = col;
                }
            }
        }

        switch (_curState)
        {
            case State.Patrol:
                if (CanSeePlayer())
                {
                    ChangeState(State.Chase);
                }
                else if (GameManager.instance.AlivePlayerCnt == 0)
                {
                    ChangeState(State.Idle);
                }
                break;

            case State.Chase:
                if (!CanSeePlayer())
                {
                    ChangeState(State.Patrol);
                }
                else if (navDistance < arrivalDist)
                {
                    ChangeState(State.Attack);
                }
                break;

            case State.Attack:
                time += Time.deltaTime;
                if (!CanSeePlayer() && time > 4)
                {
                    ChangeState(State.Patrol);
                    time = 0;
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

    private void ChangeState(State newState)
    {
        //if (_curState == newState)
        //    return;

        _curState = newState;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        switch (_curState)
        {
            case State.Idle:
                anim.SetBool("isPatrol", false);
                anim.SetBool("isChase", false);
                anim.SetBool("isAttack", false);
                break;
            case State.Patrol:
                currentCoroutine = StartCoroutine(StartPatrol());
                anim.SetBool("isPatrol", true);
                anim.SetBool("isChase", false);
                anim.SetBool("isAttack", false);
                SoundManager.instance.StopSound(soundPlayer);
                SoundManager.instance.PlaySound("FootStepNPC", true, soundPlayer);
                break;
            case State.Chase:
                currentCoroutine = StartCoroutine(chaseCoroutine);
                anim.SetBool("isPatrol", false);
                anim.SetBool("isChase", true);
                anim.SetBool("isAttack", false);
                SoundManager.instance.StopSound(soundPlayer);
                SoundManager.instance.PlaySound("FootStepNPC", true, soundPlayer);
                break;
            case State.Attack:
                currentCoroutine = StartCoroutine(attackCoroutine);
                anim.SetBool("isAttack", true);
                SoundManager.instance.PlaySound("Hunting", false, soundPlayer);
                break;
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
        nvAgent.SetDestination(wayPoints[Random.Range(0, 3)]);
        while (_curState == State.Patrol)
        {
            

            if (short_enemy != null)
            {
                yield return new WaitUntil(() => short_enemy == null);
            }

            if (navDistance < arrivalDist)
            {
                targetWayPoint = wayPoints[Random.Range(0, 3)];
                nvAgent.SetDestination(targetWayPoint);
            }

            navDistance = Vector3.Distance(this.transform.position, targetWayPoint);
            yield return null;
        }
    }

    private IEnumerator StartChase()
    {
        Debug.Log("플레이어 추격");

        while (_curState == State.Chase)
        {
            if (colliders.Length == 0)
            {
                yield return new WaitUntil(() => short_enemy != null);
            }
            if (!short_enemy.GetComponent<PlayerManager>()._isDie)
            {
                nvAgent.SetDestination(short_enemy.transform.position);
                navDistance = Vector3.Distance(this.transform.position, short_enemy.transform.position);
            }
            yield return null;
        }
    }

    private IEnumerator StartAttack()
    {
        Debug.Log("공격 중");

        while (_curState == State.Attack)
        {
            if (short_enemy != null)
            {
                short_enemy.gameObject.GetComponent<PlayerManager>().Die();
                /*
                short_enemy.GetComponent<PlayerManager>()._isDie = true;
                GameManager.instance.AlivePlayerCnt--; //공격시 생존인원  변수 감소
                GameManager.instance.check_clear();
                short_enemy.gameObject.GetComponent<CharacterController>().enabled = false; // 플레이어 맵에 존재하면 순찰 경로로 변경이 안 돼서 일단 이렇게 해놔씀
                */
                yield return new WaitForSeconds(5f) ;
            }

        }
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    
}
