using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Golem : Monster
{
    public Transform chargePoint; // 투사체를 모으는 자리

    protected override void InitStat()
    {
        _name = "Boss_Golem";
        MaxHealth = 300;
        CurHealth = 300;
        _speed = 5;
        _attackRange = 5f;
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
        AttackingTurn();


    }

    private void AttackingMove()
    {
        if (isAttackingMove) // 만약 공격 행동을 실행하여 살짝 이동해야 한다면,
        {
            transform.position += transform.forward * _speed * dir * Time.deltaTime;
        }
    }

    private void AttackingTurn()
    {
        if (isAttackingTurn) // 만약 시전 시간동안 플레이어를 쳐다봐야 한다면,
        {
            transform.LookAt(target.position);
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

    public override void MonsterSkillCharge() // 일정 타이밍마다 돌을 소환시켜준다. 또는 바닥에 떨어져있는 돌을 떙겨올 수는 없나? 또는 랜덤한 값을 줘서 각자 다른 크기의 돌을 모으자
    {
        Instantiate(chargeObject, chargePoint.transform.position, Quaternion.identity);
    }
}
