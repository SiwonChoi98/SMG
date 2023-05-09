using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSkill_GroundAttack : BaseSkill
{
    public ManualCollision monsterAttackCollision;

    [SerializeField]
    GameObject thunderEffect; // 번개 파티클

    [SerializeField]
    GameObject thunderMarkEffect; // 번개가 떨어질 자리

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = monsterAttackCollision?.CheckOverlapBox(targetMask);

        // 내려찍으면서 우선 데미지를 준다.
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(damage), effectPrefab);
        }

        StartCoroutine(SpawnThunder()); // 번개를 떨어뜨려준다.

    }

    public override void ExcuteParticleSystem()
    {

    }

    public override void ExitParticleSystem()
    {


    }

    IEnumerator SpawnThunder()
    {
        yield return new WaitForSeconds(0.5f); //0.5초 뒤부터 바닥에서 생성
        
        for(int i = 0; i < 3; i++) 
        { 
            Vector3 targetPos = GameObject.FindGameObjectWithTag("Player").transform.position; // 임시로 플레이어의 위치를 받아온다.

            GameObject thunderMark = Instantiate(thunderMarkEffect, targetPos, Quaternion.identity);

            ParticleSystem[] markParticles = thunderMark.GetComponentsInChildren<ParticleSystem>();

            foreach(ParticleSystem markParticle in markParticles)
            {
                markParticle.Play();
            }

            yield return new WaitForSeconds(0.5f); // 0.5초 정도 있다가 떨어진다. 이 부분을 늘리고 다음 스킬 시간을 줄여야할 수도 있다.

            Destroy(thunderMark); // 바닥에 보이는 표시는 삭제해준다.

            GameObject thunder = Instantiate(thunderEffect, targetPos, Quaternion.identity); // 플레이어의 위치에 생성시켜준다.

            ParticleSystem[] explosionParticles = thunder.GetComponentsInChildren<ParticleSystem>();

            foreach(ParticleSystem particle in explosionParticles) 
            {
                particle.Play(); 
            }

            yield return new WaitForSeconds(0.2f); // 번개 말고는 0.2 초 delay가 있으므로 0.2초 뒤에 영역을 생성해준다.

            thunder.GetComponent<SphereCollider>().enabled = true; // 닿으면 데미지를 주는 영역 콜라이더 활성화

            yield return new WaitForSeconds(0.5f); // 0.5초 만큼 텀을 둔다.

            Destroy(thunder, 1.4f); // 소환된 폭발을 제거해준다.

            // 현재 2.1초 동안 각 번개 유지

            
        }


    }
}
