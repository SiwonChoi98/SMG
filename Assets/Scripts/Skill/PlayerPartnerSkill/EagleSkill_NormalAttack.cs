using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSkill_NormalAttack : BaseSkill
{
    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);
    }


    public override void ExcuteParticleSystem()
    {

    }

    public override void ExitParticleSystem()
    {


    }
}
