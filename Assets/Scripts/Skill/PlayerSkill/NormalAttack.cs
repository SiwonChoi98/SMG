using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 기본 공격 1 : 24 프레임, attackforce : 0.25 / Particle : Duration : 0.5, StartDelay : 0.15, 위치는 직접 조정했음 
// 기본 공격 2 :  26 프레임, attackforce : 0.25 / Particle : Duration : 0.56, StartDelay : 0.15, 위치는 직접 조정했음 
// 기본 공격 3 :  38 프레임 , attackforce : 0.35 / Particle : Duration : 0.6, StartDelay : 0.3, 위치는 직접 조정했음

public class NormalAttack : BaseSkill
{
    public ManualCollision normalAttackCollision;

  
    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null) 
    {
        Collider[] colliders = normalAttackCollision?.CheckOverlapBox(targetMask);

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            float randDamage = Random.Range(damage, damage + damage * 0.2f);

            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)randDamage, effectPrefab);
            collider.gameObject.GetComponent<Monster>()?.KnockBack(5f); // 임시
        }
        

    }


    public override void ExcuteParticleSystem()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);

        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Play();
        }

        
    }

    public override void ExitParticleSystem()
    {
        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Stop();
        }

    }
}
