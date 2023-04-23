using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MoveState : State<Monster>
{
    public Animator animator;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        animator?.SetBool("isMove", true);
        Debug.Log("Move 상태로 진입");
        context.GetComponent<NavMeshAgent>().speed = 2.5f; //임시
    }

    public override void Update(float deltaTime) 
    {
        if (context.target)
        {
            if (context.isHit)
            {
                stateMachine.ChangeState<HitState>();
                return;
            }

            if(context.isHitEnd)
            {
                if (context.isAttackRange && context.isAttack)
                {
                    stateMachine.ChangeState<AttackState>();
                    //context.GetComponent<NavMeshAgent>().speed = 0; //임시
                    return;
                }
            }
            context.GetComponent<NavMeshAgent>().SetDestination(context.target.position);
        }
    }

    public override void OnExit()
    {
        animator?.SetBool("isMove", false);
    }
}
