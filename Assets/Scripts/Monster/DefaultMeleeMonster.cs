using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMeleeMonster : Monster
{
    protected override void InitStat() // 이 부분에서 따로 스텟을 불러와야 할듯 특히, 애니메이션 시간이 공격 쿨타임보다는 짧아야 한다., 또한 AttackRange도 몸집 크기만큼 고려해줘야 한다.
    {
        _name = "DefaultMeleeMonster";
        MaxHealth = 70;
        CurHealth = 70;
        _speed = 5;
        _attackRange = 3f; // 엘리트 몬스터 같은 경우는 크기가 있어서 어느정도 거리가 더 떨어져 있어야 한다.
        _attackTime = 2f;
        _hitTime = 1f;
        _initialAttackTime = _attackTime;
        _initialHitTime = _hitTime;
        _attackSpeed = 10f;
        
    } //임시 능력치 셋팅

    public override void Shoot()
    {
        transform.LookAt(target.position); // 때릴 때마다 플레이어 방향으로 돌려준다.
    }

    public override void MonsterSKill(int index) // 실제 공격이 나가거나 투사체가 나가야하는 타이밍, projectilePoint가 있는 경우 저기서 스킬이 나갈 것이다.
    {
        monsterSkills[index].ExcuteAttack(target.gameObject);
    }

}