using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<Monster>
{
    public Animator animator;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        context.StartCoroutine(DeadStateAnim()); // 이거 대신 죽는 시간을 체크해야 한다. 죽으면서 물리적 효과도 받으면 안된다.
    }

    public override void Update(float deltaTime)
    {
        
    }

    public override void OnExit()
    { 

    }

    IEnumerator DeadStateAnim()
    {
        animator?.SetTrigger("doDead");

        context.gameObject.layer = 14; // MonsterDeath 레이어로 바꿔준다.

        if(context.mType != EMonsterType.Common) // 일반 몬스터가 아닌 경우엔 몬스터 콜라이더도 바꿔준다.
        {
            context.transform.Find("MonsterCollider").gameObject.layer = 14;
        }
       
        yield return new WaitForSeconds(1.4f);

        context.GetComponent<DropItems>().DropItem(); // 몬스터 타입에 맞게 아이템을 떨어뜨려준다.

        yield return new WaitForSeconds(0.2f);

        context.gameObject.SetActive(false);

        // context.gameObject.transform.SetParent() : 나중에 아이템 떄문에 렉걸리면 풀링으로 처리하도록 합시다.

        context.gameObject.layer = 7; // Monster 레이어로 바꿔준다.

    }



}
