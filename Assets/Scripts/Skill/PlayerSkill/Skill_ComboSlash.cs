﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ComboSlash : BaseSkill
{
    [SerializeField]
    GameObject slashObject;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        SoundManager.instance.SfxPlaySound(14, 0.3f);

        GameObject go = Instantiate(slashObject,
            SkillManager.instance.player.skillSpawnPos[(int)ESkillType.ComboSlash].position + Vector3.up * 1.5f, // 살짝 위에서 날라가야 한다.
             SkillManager.instance.player.skillSpawnPos[(int)ESkillType.ComboSlash].rotation);

        CameraShake.instance.OnShakeCamera(0.1f, 0.5f);

        go.GetComponent<Projectile_ComboSlash>().SetDamage((int)(damage));
        go.GetComponent<Projectile_ComboSlash>().SetTarget(targetMask);
        go.GetComponent<Projectile_ComboSlash>().SetHitEffect(effectPrefab);
    }

    public override void ExcuteParticleSystem()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);

        SoundManager.instance.SfxPlaySound(15);

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

    }

    public override void ExitParticleSystem()
    {


    }
}
