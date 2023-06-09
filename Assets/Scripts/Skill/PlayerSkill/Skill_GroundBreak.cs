﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_GroundBreak : BaseSkill
{
    public ManualCollision groundBreakAttackCollision; // 나중에 스킬이 늘어나면 배열로 바꿔줘서, ExcuteAttack에 강화정도의 인덱스를 넣어준다. 그 인덱스


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        SoundManager.instance.SfxPlaySound(10);

        ManualCollision collision = GameObject.Find("GroundBreak_Area").GetComponent<ManualCollision>(); // 만약 할당이 안된 상태라면 저장해준다.

        groundBreakAttackCollision = collision;

        Collider[] colliders = groundBreakAttackCollision?.CheckOverlapBox(targetMask);

        CameraShake.instance.OnShakeCamera(0.1f, 0.6f);

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            float randDamage = Random.Range(damage, damage + damage * 0.2f);

            collider.gameObject.GetComponent<Monster>()?.SetHitBySkill(true);
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(randDamage), effectPrefab);
            collider.gameObject.GetComponent<Monster>()?.KnockBack(2f);  // 임시

        }
    }

    public override void ExcuteParticleSystem()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

        SoundManager.instance.SfxPlaySound(11, 0.7f);
    }

    public override void ExitParticleSystem()
    {


    }

}
