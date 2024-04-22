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

    private PlayerControl playerControl;
    private GameObject player;



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
        player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        Debug.Log("init");
        
        nvAgent.destination = transform.position;
        wayPoints = new[] { new Vector3(-40, this.transform.position.y, -40), new Vector3(40f, this.transform.position.y, 40f), new Vector3(0, this.transform.position.y, 0) };

        patrolCoroutine = StartPatrol();
        chaseCoroutine = StartChase();


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
                    _curState = State.Idle;
                    break;
                }
                break;
            case State.Attack:
                break;
            case State.Idle:
                StopAllCoroutines();
                break;
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



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}