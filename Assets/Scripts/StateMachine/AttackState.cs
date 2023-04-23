using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : State<Monster>
{
    public Animator animator;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        //context.GetComponent<NavMeshAgent>().isStopped = true;
        //context.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        context.GetComponent<NavMeshAgent>().speed = 0;
        //context.anim.SetBool("SkillEnd", false); 
        Debug.Log("Attack 상태로 진입");
    }

    public override void Update(float deltaTime)
    {
        if (context.target)
        {
            if (context.isHit) // 플레이어에게 맞으면 무조건 Hit, 몬스터 종류에 따라 안넘어갈수도 있다.
            {
                context.anim.SetBool("SkillEnd", true); // 하던 공격을 취소해준다.
                stateMachine.ChangeState<HitState>();
                return;
            }
            if (!context.isAttackRange && context.anim.GetBool("SkillEnd")) // 공격 사거리 내에 들어와있지 않다면 Idle로 이동 && 공격 애니메이션이 다 끝나면 Idle로 이동
            {
                stateMachine.ChangeState<IdleState>();
                return;
            }
            if (context.isAttack) //공격할수있으면 다시 공격
            {
                context.anim.SetBool("SkillEnd", false);
                animator?.SetTrigger("doAttack");
                context.Shoot();
                Debug.Log("Attack 성공");
                context.isAttack = false;
                return;
            }
        }

    }

    public override void OnExit()
    {
        //context.GetComponent<NavMeshAgent>().isStopped = false;
        context.anim.SetBool("SkillEnd", true);
        context.isAttack = false;
    }
}
