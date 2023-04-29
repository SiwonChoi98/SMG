using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Wolf : Monster
{
    protected override void InitStat() 
    {
        _name = "Boss_Wolf";
        MaxHealth = 300;
        CurHealth = 300;
        _speed = 5;
        _attackRange = 3f;
        _attackTime = 2.5f;
        _hitTime = 4f;
        _initialAttackTime = _attackTime;
        _initialHitTime = _hitTime;
        _attackSpeed = 10f;
    } //임시 능력치 셋팅

    protected override void Update()
    {
        base.Update();

        AttackingMove();

    }

    private void AttackingMove()
    {
        if (isAttackingMove) // 만약 공격 행동을 실행하여 살짝 이동해야 한다면,
        {
            transform.position += transform.forward * _speed *  dir * Time.deltaTime;
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
