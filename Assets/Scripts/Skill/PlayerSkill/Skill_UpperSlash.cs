using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_UpperSlash : BaseSkill // 0.5 정도로 attackFoce 주자.
{
    [SerializeField]
    GameObject slashObject;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        SoundManager.instance.SfxPlaySound(7, 0.5f);

        GameObject go = Instantiate(slashObject, 
            SkillManager.instance.player.skillSpawnPos[(int)ESkillType.UpperSlash].position,
             SkillManager.instance.player.skillSpawnPos[(int)ESkillType.UpperSlash].rotation);

        CameraShake.instance.OnShakeCamera(0.1f, 0.6f);

        go.GetComponent<Projectile_UpperSlash>().SetDamage((int)(damage));
        go.GetComponent<Projectile_UpperSlash>().SetTarget(targetMask);
        go.GetComponent<Projectile_UpperSlash>().SetHitEffect(effectPrefab);

    }

    public override void ExcuteParticleSystem()
    {

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);
    }

    public override void ExitParticleSystem()
    {


    }
}
