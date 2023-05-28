using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MoveState : State<Monster>
{
    public Animator animator;

    public float randPosX;
    public float randPosZ;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        animator?.SetBool("isMove", true);
        Debug.Log("Move 상태로 진입");

        randPosX = Random.Range(-0.7f, 0.7f);
        randPosZ = Random.Range(-0.7f, 0.7f);

        // 이 부분을 context.Speed로 바꿔줘야한다.
        context.Speed = Random.Range(context._initialSpeed - 0.5f, context._initialSpeed + 0.5f); // MoveState 진입 시 Speed에 차이를 준다.
        //context.GetComponent<NavMeshAgent>().speed = 2.5f; 
    }

    public override void Update(float deltaTime) 
    {
        if (GameManager.instance.stage.asset.IsClear())
        {
            stateMachine.ChangeState<DeadState>();
            return;
        }
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
            
            context.rigid.MovePosition(Vector3.MoveTowards(context.transform.position, context.target.position + new Vector3(randPosX, 0, randPosZ) , Time.deltaTime * context.Speed));
            //context.transform.position = Vector3.MoveTowards(context.transform.position, context.target.position, Time.deltaTime * 2.5f);
            context.transform.LookAt(context.target.position + new Vector3(randPosX, 0, randPosZ));
            context.rigid.velocity = Vector3.zero;
        }
    }

    public override void OnExit()
    {
        animator?.SetBool("isMove", false);
    }
}
