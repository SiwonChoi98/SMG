using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Baldo : BaseSkill // 89 프레임, 12프레임부터 파티클 재생, 1초 정도 있다가 칼날 생성, 50프레임정도에 칼 떨어지면서 생성, 
{
    // Start is called before the first frame update

    public ManualCollision BaldoAttackCollision; // 나중에 스킬이 늘어나면 배열로 바꿔줘서, ExcuteAttack에 강화정도의 인덱스를 넣어준다. 그 인덱스

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        ManualCollision collision = GameObject.Find("Baldo_Area").GetComponent<ManualCollision>(); // 만약 할당이 안된 상태라면 저장해준다.

        BaldoAttackCollision = collision;


        Collider[] colliders = BaldoAttackCollision?.CheckOverlapBox(targetMask);

        if (colliders.Length > 0)  // 타격 시에만 이펙트 처리
        {
            CameraShake.instance.OnShakeCamera(0.05f, 0.5f);
        }

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            float randDamage = Random.Range(damage, damage + damage * 0.2f);

            collider.gameObject.GetComponent<Monster>()?.SetHitBySkill(true);
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(randDamage), effectPrefab);
        }
    }

    public override void ExcuteParticleSystem()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        damage = (int)(player.Strength * damageMult);

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

        SoundManager.instance.SfxPlaySound(4);
    }

    public override void ExitParticleSystem()
    {


    }
}
