using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ElderlyController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    public Transform targetPoint; // 設定目標點

    private bool hasArrived = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // 設定目標點並開始移動
        agent.SetDestination(targetPoint.position);
        animator.SetFloat("Speed", 1f); // 設定行走動畫
    }

    void Update()
    {
        // 檢查是否已經到達目標點
        if (!hasArrived && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasArrived = true;
            animator.SetFloat("Speed", 0f); // 停止移動，進入 Idle
        }
    }
}

