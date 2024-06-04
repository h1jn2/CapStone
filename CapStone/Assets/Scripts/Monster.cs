using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
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

    private int playerCount;
    private int attackCount;



    private enum State
    {
        Patrol,
        Chase,
        Attack,
        Idle
    }

    [SerializeField]
    private State _curState;

    private void Start()
    {
        _curState = State.Patrol;
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        playerCount = GameObject.FindGameObjectsWithTag("Player").Length;

        nvAgent.destination = transform.position;
        wayPoints = new[] { new Vector3(-30, this.transform.position.y, 110), new Vector3(-30, this.transform.position.y, 130) };

        patrolCoroutine = StartPatrol();
        chaseCoroutine = StartChase();
        attackCoroutine = StartAttack();


    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

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
                break;
            case State.Chase:
                StartCoroutine(chaseCoroutine);

                if (!CanSeePlayer())
                {
                    _curState = State.Patrol;
                    break;
                }
                if (navDistance < arrivalDist)
                {
                    _curState = State.Attack;
                    break;
                }
                break;
            case State.Attack:
                StartCoroutine(attackCoroutine);
                _curState = State.Patrol;
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
        if (colliders.Length > 0)
            return true;
        else
            return false;
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
            navDistance = nvAgent.remainingDistance;
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
                navDistance = nvAgent.remainingDistance;
            }
            yield return null;
        }
    }

    private IEnumerator StartAttack()
    {
        Debug.Log("공격 중");

        if (GameManager.instance._currentStatus != GameManager.Status._end)
        {
            GameManager.instance._currentStatus = GameManager.Status._end;

        }
        Debug.Log(GameManager.instance._currentStatus);
        yield return null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
