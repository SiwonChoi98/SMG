using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSkill_NormalAttack : BaseSkill
{
    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);

        float randDamage = Random.Range(damage, damage + damage * 0.2f);

        target.GetComponent<IDamageable>()?.TakeDamage((int)randDamage, effectPrefab);
    }


    public override void ExcuteParticleSystem()
    {

    }

    public override void ExitParticleSystem()
    {


    }
}
