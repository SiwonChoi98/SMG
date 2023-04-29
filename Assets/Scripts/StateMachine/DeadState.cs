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
        yield return new WaitForSeconds(1.6f);
        GameObject.Destroy(context.gameObject);
    }
}
