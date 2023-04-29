using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HitState : State<Monster>
{
    public Animator animator;
    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        //context.GetComponent<NavMeshAgent>().speed = 0;
        Debug.Log("Hit 상태로 진입");
        animator?.SetTrigger("doHit");

        context.isHitEnd = false; // HitState로 변할 때 마다 초기화
        context._hitTime = context._initialHitTime; // 맞을 때 마다 초기화
        context.transform.LookAt(context.target.position); // 맞을 때 마다 플레이어 방향으로 틀어줌
    }

    public override void Update(float deltaTime)
    {   
        if(context.CurHealth > 0) // 여기에 애니메이션 이벤트 추가해서 작업하는 것이 어떤지, Hit 모션이 다 끝나면 Idle로 가도록
        {
            stateMachine.ChangeState<IdleState>();
        }
    }

    public override void OnExit()
    {

        //context.GetComponent<NavMeshAgent>().isStopped = false; 
        context.isHit = false;
        
    }
    
}