using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IdleState : State<Monster>
{
    public Animator animator;

    public override void OnInitialized() //셋팅
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter() //한번실행
    {
        context.transform.LookAt(context.target.position);
        //context.GetComponent<NavMeshAgent>().speed = 0f; //임시
        animator?.SetBool("isIdle", true);

        Debug.Log("idle 상태로 진입");

    }

    public override void Update(float deltaTime) //게속업데이트
    {
        if (context.target)
        {
            if (context.isHit)
            {
                stateMachine.ChangeState<HitState>();
                return;
            }

            if(context.isHitEnd) // 맞는 상태가 종료된 시, 즉 다 맞았거나, 맞지 않은 상태에서
            {
                if (context.isAttackRange && context.isAttack) // 공격이 사거리에 들어오면 이동 && 공격 쿨타임이 다 찼으면 이동
                {
                    stateMachine.ChangeState<AttackState>();
                    return;
                }
                stateMachine.ChangeState<MoveState>();
            }
        }
    }

    public override void OnExit() //나가기
    {
        animator?.SetBool("isIdle", false);
    }
}
