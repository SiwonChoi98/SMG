using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SpawnEagle : BaseSkill
{
    [SerializeField]
    GameObject eagle;


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {

        GameObject go = Instantiate(eagle,
           SkillManager.instance.player.skillSpawnPos[(int)ESkillType.SpawnEagle].position,
            SkillManager.instance.player.skillSpawnPos[(int)ESkillType.SpawnEagle].rotation
            );

    }

    public override void ExcuteParticleSystem()
    {

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

        SoundManager.instance.SfxPlaySound(13, 0.5f);
    }

    public override void ExitParticleSystem()
    {


    }
}
