using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;

public class MonsterController : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private const float ATTACK_RANGE = 2f;

    [Header("Navigation Move")] [SerializeField]
    private Transform player;

    [Tooltip("몬스터가 플레이어를 인지하는 사정거리")] [SerializeField]
    private float chaseDistance;

    [Header("Animation")] [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private readonly WaitForSeconds coroutineWaitTime = new WaitForSeconds(0.5f);
    private float navDistance;
    private AnimatorStateInfo animInfo;
    private bool isDead = false;
    private Rigidbody rigidbody;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(GetPathDistanceCoroutine());
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isDead)
        {
            animInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (navDistance <= ATTACK_RANGE)
            {
                AttackMonster();
            }
            else
            {
                MoveMonster();
            }
        }
    }

    private void MoveMonster()
    {
        if (navDistance <= chaseDistance)
        {
            agent.SetDestination(player.position);
            animator.SetBool(Walk, true);
        }
        else
        {
            agent.ResetPath(); // 너무 멀면 멈춤
            animator.SetBool(Walk, false);
        }
    }

    private void AttackMonster()
    {
        if (!animInfo.IsName("Attack"))
        {
            agent.ResetPath();
            animator.SetBool(Walk, false);

            transform.LookAt(player);

            animator.ResetTrigger(Attack);
            animator.SetTrigger(Attack);
        }
    }

    private float GetPathDistance(Vector3 start, Vector3 end)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path))
        {
            float distance = 0f;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return distance;
        }

        return Mathf.Infinity;
    }

    private IEnumerator GetPathDistanceCoroutine()
    {
        while (true)
        {
            navDistance = GetPathDistance(transform.position, player.position);

            yield return coroutineWaitTime;
        }
    }

    public void PlayDead()
    {
        animator.SetTrigger(Dead);
        agent.enabled = false;
        rigidbody.velocity = Vector3.zero;
        isDead = true;
    }
}