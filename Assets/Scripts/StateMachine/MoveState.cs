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
        // 이 부분을 context.Speed로 바꿔줘야한다.
        //context.GetComponent<NavMeshAgent>().speed = 2.5f; 
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
                if (context.isAttackRange)
                {
                    stateMachine.ChangeState<AttackState>();
                    //context.GetComponent<NavMeshAgent>().speed = 0; //임시
                    return;
                }
            }
            context.rigid.MovePosition(Vector3.MoveTowards(context.transform.position, context.target.position, Time.deltaTime * 2.5f));
            //context.transform.position = Vector3.MoveTowards(context.transform.position, context.target.position, Time.deltaTime * 2.5f);
            context.transform.LookAt(context.target.position);
        }
    }

    public override void OnExit()
    {
        animator?.SetBool("isMove", false);
    }
}
