using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRangeMonster : Monster
{
    protected override void InitStat()
    {
        _name = "DefaultRangeMonster";
        MaxHealth = 30;
        CurHealth = 30;
        _speed = 5;
        _attackRange = 6f;
        _attackTime = 3f;
        _hitTime = 1.5f;
        _initialAttackTime = _attackTime;
        _initialHitTime= _hitTime;
        _attackSpeed = 10f;
    } //임시 능력치 셋팅

    public override void Shoot()
    {

        transform.LookAt(target.position);
        GameObject gameObject = Instantiate(attackPrefab);
        gameObject.transform.position = transform.position + new Vector3(0, 1, 0);
        gameObject.transform.rotation = gameObject.transform.rotation;
        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * _attackSpeed;
        Destroy(gameObject, 3f);
    } //임시) 원거리공격

    public void MonsterSKill() // 실제 공격이 나가거나 투사체가 나가야하는 타이밍
    {
        monsterSkills[0].ExcuteAttack(target.gameObject); // 여기서 몬스터의 첫번째 공격스킬이 나간다. ex) 고블린 평타
    }

    public void MonsterSkillEnd() // 공격 애니메이션이 다 끝났는지 확인함
    {
        anim.SetBool("SkillEnd", true); // true로 만들어주면 attack motion 탈출
    }
}
