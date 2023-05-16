using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill_NormalAttack : BaseSkill // 이거는 진짜 평타만 치는 일반 몬스터에게 적용, 엘리트 몬스터는 다른 스킬을 만들어야 함
{
    public ManualCollision monsterAttackCollision;


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = monsterAttackCollision?.CheckOverlapBox(targetMask);

        
        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            float randDamage = Random.Range(damage, damage + (damage * 0.2f));

            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(randDamage), effectPrefab);
        }


    }


    public override void ExcuteParticleSystem()
    {
       
    }

    public override void ExitParticleSystem()
    {
       

    }
}
