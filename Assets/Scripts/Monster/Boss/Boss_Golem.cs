using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Golem : Monster
{
    public Transform chargePoint; // 투사체를 모으는 자리
    public Transform barrierPoint; // 바닥에서 나오는 파티클 위치
    public Transform groundAttackPoint; // 바닥을 칠 때 나오는 파티클 위치

    public SphereCollider barrierCollider; // 베리어 생성 시 플레이어를 밀어냄

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

    public override void MonsterSkillCharge() // 일정 타이밍마다 돌을 소환시켜준다. 또는 바닥에 떨어져있는 돌을 떙겨올 수는 없나? 또는 랜덤한 값을 줘서 각자 다른 크기의 돌을 모으자
    {
        int skillNum = anim.GetInteger("SkillNumber");

        
        if(skillNum == 1) // 바닥에서 패턴 나오는 공격이라면
        {
            StartCoroutine(ChargeGroundAttack());
        }
        else if (skillNum == 2) // 돌 던지는 공격이라면
        {
            StartCoroutine(ChargeRocks()); // 기를 모아준다.
            StartCoroutine(ChargeBarrier()); // 장벽을 생성해준다. 장벽을 생성하면서 플레이어를 넉백시킨다.
        }

    }

    IEnumerator ChargeRocks() // 돌을 모아준다.
    {
        anim.speed = 0f; // 잠깐 0으로 만들어준다.

        // 여기서 파티클을 해당 위치에 재생시킨다.

        ParticleSystem[] throwParticles = chargePoint.GetComponentsInChildren<ParticleSystem>(); // 던지는 부분에 파티클 생성

        foreach (ParticleSystem particle in throwParticles) 
        {
            particle.Play();
        }

        yield return new WaitForSeconds(3f); // 3초 뒤에는 다시 애니메이션을 실행시켜준다.

        anim.speed = 1f; 

    }

    IEnumerator ChargeBarrier() // 베리어 영역 1을 켜준다.
    {
        ParticleSystem[] barrierParticles = barrierPoint.GetComponentsInChildren<ParticleSystem>(); // 바닥 부분에 파티클 생성

        foreach (ParticleSystem particle in barrierParticles)
        {
            particle.Play();
        }

        barrierCollider.enabled = true; // 베리어 부분의 박스 콜라이더를 켜준다.

        yield return new WaitForSeconds(3.5f);

        barrierCollider.enabled = false;

    }

    IEnumerator ChargeGroundAttack() // 땅을 치면서 파티클을 생성시켜준다.
    {
        anim.speed = 0f;

        ParticleSystem[] groundAttackParticles = groundAttackPoint.GetComponentsInChildren<ParticleSystem>(); // 던지는 부분에 파티클 생성

        foreach (ParticleSystem particle in groundAttackParticles)
        {
            particle.Play();
        }

        yield return new WaitForSeconds(3f); // 3초 뒤에는 다시 애니메이션을 실행시켜준다.

        anim.speed = 1f;

        
    }
}
