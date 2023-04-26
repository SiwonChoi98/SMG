using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMeleeMonster : Monster
{
    protected override void InitStat() // 이 부분에서 따로 스텟을 불러와야 할듯 특히, 애니메이션 시간이 공격 쿨타임보다는 짧아야 한다.
    {
        _name = "DefaultMeleeMonster";
        MaxHealth = 70;
        CurHealth = 70;
        _speed = 5;
        _attackRange = 3f;
        _attackTime = 2f;
        _hitTime = 1f;
        _initialAttackTime = _attackTime;
        _initialHitTime = _hitTime;
        _attackSpeed = 10f;
        monsterSkills.Add(this.GetComponentInChildren<MonsterSkill_NormalAttack>()); //몬스터 공격 스크립트 추가
    } //임시 능력치 셋팅

    public override void Shoot()
    {
        transform.LookAt(target.position); // 때릴 때마다 플레이어 방향으로 돌려준다.
    }

    //public void MonsterSKill() // 실제 공격이 나가거나 투사체가 나가야하는 타이밍
    //{
    //    monsterSkills[0].ExcuteAttack(target.gameObject); // 여기서 몬스터의 첫번째 공격스킬이 나간다. ex) 고블린 평타
    //}

    //public void MonsterSkillEnd() // 공격 애니메이션이 다 끝났는지 확인함
    //{
    //    anim.SetBool("SkillEnd", true); // true로 만들어주면 attack motion 탈출
    //}


}