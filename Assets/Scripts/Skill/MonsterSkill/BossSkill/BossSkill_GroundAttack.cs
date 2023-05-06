using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill_GroundAttack : BaseSkill
{
    public ManualCollision monsterAttackCollision;

    [SerializeField]
    GameObject summonExplosion;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = monsterAttackCollision?.CheckOverlapBox(targetMask);

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(damage), effectPrefab);
        }

        StartCoroutine(SpawnExplosion()); // 바닥에서 폭발을 생성시켜준다.

    }


    public override void ExcuteParticleSystem()
    {

    }

    public override void ExitParticleSystem()
    {


    }

    IEnumerator SpawnExplosion()
    {
        yield return new WaitForSeconds(0.5f); //0.5초 뒤부터 바닥에서 생성
        
        for(int i = 0; i < 3; i++) 
        { 
            Vector3 targetPos = GameObject.FindGameObjectWithTag("Player").transform.position; // 임시로 플레이어의 위치를 받아온다.

            GameObject explosion = Instantiate(summonExplosion, targetPos, Quaternion.identity); // 플레이어의 위치에 생성시켜준다.

            ParticleSystem[] explosionParticles = explosion.GetComponentsInChildren<ParticleSystem>();

            foreach(ParticleSystem particle in explosionParticles) 
            {
                particle.Play(); 
            }

            yield return new WaitForSeconds(0.7f); // 0.7초 만큼 텀을 둔다.
        }


    }
}
