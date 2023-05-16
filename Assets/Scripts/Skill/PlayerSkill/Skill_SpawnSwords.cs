﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SpawnSwords : BaseSkill
{
    [SerializeField]
    GameObject swordObject;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        for(int i = 0; i < 3; i++) // 검을 3번 소환한다.
        {
            GameObject go = Instantiate(swordObject,
            SkillManager.instance.player.skillSpawnPos[(int)ESkillType.SpawnSwords].position, // 적이 여려명이면 괜찮
            Quaternion.identity
            );

            go.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse); // 순간적으로 위로 퉁 올림

            int RandDamage = Random.Range(19, 25);

            go.GetComponent<Projectile_SpawnSwords>().SetDamage((int)(RandDamage));
            go.GetComponent<Projectile_SpawnSwords>().SetTarget(targetMask);
            go.GetComponent<Projectile_SpawnSwords>().SetHitEffect(effectPrefab);
        }
        

    }

    public override void ExcuteParticleSystem()
    {

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

        SoundManager.instance.SfxPlaySound(11, 0.5f);
    }

    public override void ExitParticleSystem()
    {


    }

    IEnumerator SpawnSword()
    {
        yield return new WaitForSeconds(1);

       

       
    }
}
