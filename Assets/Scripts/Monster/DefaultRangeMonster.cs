using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRangeMonster : Monster
{
    protected override void InitStat()
    {
        //_name = "DefaultRangeMonster";
        //MaxHealth = 70;
        //CurHealth = 70;
        //_speed = 5;
        //_attackRange = 7f;
        //_attackTime = 2.5f;
        //_hitTime = 1.5f;
        //_initialAttackTime = _attackTime;

        _hitTime = _initialHitTime;
        //_attackSpeed = 10f;
    } //임시 능력치 셋팅

    public override void Shoot()
    {

        transform.LookAt(target.position);
        //GameObject gameObject = Instantiate(attackPrefab);
        //gameObject.transform.position = transform.position + new Vector3(0, 1, 0);
        //gameObject.transform.rotation = gameObject.transform.rotation;
        //Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        //rigid.velocity = transform.forward * _attackSpeed;
        //Destroy(gameObject, 3f);
    } //임시) 원거리공격

    public override void MonsterSKill(int index) // 실제 공격이 나가거나 투사체가 나가야하는 타이밍, projectilePoint가 있는 경우 저기서 스킬이 나갈 것이다.
    {
        if(projectilePoint != null)
        {
            monsterSkills[index].ExcuteAttack(target.gameObject, projectilePoint);
        }
    }
}
