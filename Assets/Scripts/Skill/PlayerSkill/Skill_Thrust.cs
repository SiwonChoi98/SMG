﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 기본 28프레임, attackforce : 0.5, 3~14정도까지 찌르기동작, 12나 13정도에 파티클 생성 Duration 0.3 , StartDelay : 0.43, 
public class Skill_Thrust : BaseSkill
{
    // Start is called before the first frame update

    public ManualCollision thrustAttackCollision;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        ManualCollision collision = GameObject.Find("Thrust_Area").GetComponent<ManualCollision>(); // 만약 할당이 안된 상태라면 저장해준다.

        thrustAttackCollision = collision;

        Collider[] colliders = thrustAttackCollision?.CheckOverlapBox(targetMask);

        if (colliders.Length > 0)  // 타격 시에만 이펙트 처리
        {
            CameraShake.instance.OnShakeCamera(0.1f, 0.6f);
        }

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            float randDamage = Random.Range(damage, damage + damage * 0.2f);

            collider.gameObject.GetComponent<Monster>()?.SetHitBySkill(true);
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(randDamage), effectPrefab);
            collider.gameObject.GetComponent<Monster>()?.KnockBack(25f);  // 임시 이정도는 해야 좀 밀린다.
        }


    }


    public override void ExcuteParticleSystem()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);

        SkillManager.instance.AttachParticle(mSkillType, mParticleType);

        SoundManager.instance.SfxPlaySound(5);
    }

    public override void ExitParticleSystem()
    {
   
    }
}
