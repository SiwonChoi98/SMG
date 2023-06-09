﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 기본 48프레임, 6프레임까지 이동, 34까지 베기, 45까지 이동, 9정도에 생성 Duration : 4, StartDelay : 0.3
public class Skill_GiantSword : BaseSkill
{
    // Start is called before the first frame update

    public ManualCollision giantSwordAttackCollision; // 나중에 스킬이 늘어나면 배열로 바꿔줘서, ExcuteAttack에 강화정도의 인덱스를 넣어준다. 그 인덱스

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        SoundManager.instance.SfxPlaySound(9, 0.5f);

        ManualCollision collision = GameObject.Find("GiantSword_Area").GetComponent<ManualCollision>(); // 만약 할당이 안된 상태라면 저장해준다.

        giantSwordAttackCollision = collision;

        Collider[] colliders = giantSwordAttackCollision?.CheckOverlapBox(targetMask);

        CameraShake.instance.OnShakeCamera(0.1f, 0.7f);

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            float randDamage = Random.Range(damage, damage + damage * 0.2f);

            collider.gameObject.GetComponent<Monster>()?.SetHitBySkill(true);
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(randDamage), effectPrefab);
            
        }
    }

    public override void ExcuteParticleSystem()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

        SoundManager.instance.SfxPlaySound(8, 0.5f);
    }

    public override void ExitParticleSystem()
    {   


    }
}