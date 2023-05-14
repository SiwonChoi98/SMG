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
        //context.GetComponent<NavMeshAgent>().speed = 0;
        //context.anim.SetBool("SkillEnd", false); 

        Debug.Log("Attack 상태로 진입");
    }

    public override void Update(float deltaTime)
    {
        if (context.target)
        {
            
            if(context.mType == EMonsterType.Boss && context.isAttack) // 만약 보스 타입이라면 && 그리고 공격할 수 있으면
            {

                float ranNum = Random.Range(0f, 10f);

                if(ranNum >= 0 && ranNum <= 5f) // 50퍼센트
                {
                    context.anim.SetBool("SkillEnd", false); 
                    animator.SetTrigger("doAttack"); // 공격을 시키고
                    animator.SetInteger("SkillNumber", 2); // 첫번째 스킬 애니메이션을 실행해준다.
                    context.Shoot();
                    context.isAttack = false;
                }

                else if(ranNum > 5f && ranNum <= 8.5f) // 35퍼센트
                {
                    context.anim.SetBool("SkillEnd", false);
                    animator.SetTrigger("doAttack"); // 공격을 시키고
                    animator.SetInteger("SkillNumber", 2); // 두번째 스킬 애니메이션을 실행해준다.
                    context.Shoot();
                    context.isAttack = false;
                }

                else // 15퍼센트
                {
                    // 이 부분을 수정해서 잠시동안 애니메이션이 멈추게 해보자.
                    context.anim.SetBool("SkillEnd", false);
                    animator.SetTrigger("doAttack"); // 공격을 시키고
                    animator.SetInteger("SkillNumber", 2); // 세번째 스킬 애니메이션을 실행해준다.
                    context.Shoot();
                    context.isAttack = false;
                }
            }

            if (context.isHit) // 플레이어에게 맞으면 무조건 Hit, 몬스터 종류에 따라 안넘어갈수도 있다.
            {
                context.anim.SetBool("SkillEnd", true); // 하던 공격을 취소해준다.
                stateMachine.ChangeState<HitState>();
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
            if (!context.isAttackRange && context.anim.GetBool("SkillEnd")) // 공격 사거리 내에 들어와있지 않다면 Idle로 이동 && 공격 애니메이션이 다 끝나면 Idle로 이동
            {
                stateMachine.ChangeState<IdleState>();
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
