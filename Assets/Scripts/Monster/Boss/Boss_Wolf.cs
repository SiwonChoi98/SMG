using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Wolf : Monster
{
    protected override void InitStat() 
    {

    } //임시 능력치 셋팅

    protected override void Update()
    {
        base.Update();

        AttackingMove();
        AttackingTurn();
    }

    private void AttackingMove()
    {
        if (isAttackingMove) // 만약 공격 행동을 실행하여 살짝 이동해야 한다면,
        {
            transform.position += transform.forward * _speed *  dir * Time.deltaTime;
        }
    }

    private void AttackingTurn()
    {
        if (isAttackingTurn) // 만약 시전 시간동안 플레이어를 쳐다봐야 한다면,
        {
            Vector3 t_dir = (target.position - transform.position).normalized;
            Quaternion from = transform.rotation;

            Quaternion to = Quaternion.LookRotation(t_dir);

            transform.rotation = Quaternion.Lerp(from, to, Time.fixedDeltaTime * 2f);
        }
    }
    public override void Shoot()
    {
        transform.LookAt(target.position);
    } 

    public override void MonsterSKill(int index) // 보스는 공격이 여러가지므로 각각에 맞게 ExcuteAttack을 실행시켜준다.
    {
        monsterSkills[index].ExcuteAttack(target.gameObject);
    }
}
