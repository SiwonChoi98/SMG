﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스킬 매니저는 오직 플레이어의 스킬 시스템을 위해 존재하는 매니저
// skills와 skillSpawnPos는 ESkill과 일치해야한다. 


public class SkillManager : MonoBehaviour
{   // 스킬 매니저의 역할에 대해서,
    public static SkillManager instance;

    public Player player;

    public GameObject[] playerSkills;

    public GameObject[] scrollParticle; // 스크롤 획득 시 틀어주는 이펙트

    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Update()
    {
        if (player.ShieldCount <= 0 && player.isShield)  // 쉴드 카운트가 0이 된 경우 && 그전까지 쉴드 상태였던 경우
        {
            Debug.Log("쉴드 상태 제거");
            player.isShield = false;
            GameObject shield = GameObject.Find("Shield1(Clone)"); // 일단 임시로 이렇게 제거하도록 하였다.
            Destroy(shield);
        }
    }

    public void SpawnParticle(ESkillType skillType, ESkillParticleType particleType) // 레벨은 나중에 넣든지 하자.
    {
        if (particleType == ESkillParticleType.Spawn) // 만약 타입이 스폰형 스킬이라면
        {
            // 자이언트 소드 소환 스킬
            if (skillType == ESkillType.GiantSword) // 이 아래에 Switch (level)에  따라 다른 파티클 시스템이 나갈 예정
            {
                GameObject GiantSwordSkill = Instantiate(playerSkills[(int)ESkillType.GiantSword], 
                        player.skillSpawnPos[(int)ESkillType.GiantSword].position,
                        player.skillSpawnPos[(int)ESkillType.GiantSword].rotation); 

                ParticleSystem[] particleSystems = GiantSwordSkill.GetComponentsInChildren<ParticleSystem>();  

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 파티클 재생
                }
                Destroy(GiantSwordSkill, 4f);
            }
            
            // 땅으로 내려찍는 스킬
            else if (skillType == ESkillType.GroundBreak)
            {
                GameObject GroundBreakSkill = Instantiate(playerSkills[(int)ESkillType.GroundBreak], 
                        player.skillSpawnPos[(int)ESkillType.GroundBreak].position,
                        player.skillSpawnPos[(int)ESkillType.GroundBreak].rotation); 

                ParticleSystem[] particleSystems = GroundBreakSkill.GetComponentsInChildren<ParticleSystem>(); 

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }
                Destroy(GroundBreakSkill, 3f);

            }

            else if (skillType == ESkillType.UpperSlash)
            {
                GameObject UpperSlashSkill = Instantiate(playerSkills[(int)ESkillType.UpperSlash],
                        player.skillSpawnPos[(int)ESkillType.UpperSlash].position,
                        player.skillSpawnPos[(int)ESkillType.UpperSlash].rotation);

                ParticleSystem[] particleSystems = UpperSlashSkill.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(UpperSlashSkill, 3f);
            }

            else if (skillType == ESkillType.Baldo)
            {
                GameObject BaldoSkill = Instantiate(playerSkills[(int)ESkillType.Baldo],
                       player.skillSpawnPos[(int)ESkillType.Baldo].position,
                       player.skillSpawnPos[(int)ESkillType.Baldo].rotation);

                ParticleSystem[] particleSystems = BaldoSkill.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(BaldoSkill, 3f);
            }

            else if (skillType == ESkillType.SpawnEagle)
            {
                GameObject SpawnEagleSkill = Instantiate(playerSkills[(int)ESkillType.SpawnEagle],
                       player.skillSpawnPos[(int)ESkillType.SpawnEagle].position,
                       player.skillSpawnPos[(int)ESkillType.SpawnEagle].rotation);


                ParticleSystem[] particleSystems = SpawnEagleSkill.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(SpawnEagleSkill, 1.1f);

            }

            else if (skillType == ESkillType.ComboSlash)
            {
                GameObject ComboSlashSkill = Instantiate(playerSkills[(int)ESkillType.ComboSlash],
                       player.skillSpawnPos[(int)ESkillType.ComboSlash].position,
                       player.skillSpawnPos[(int)ESkillType.ComboSlash].rotation);


                ParticleSystem[] particleSystems = ComboSlashSkill.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(ComboSlashSkill, 5f);

            }

            else if (skillType == ESkillType.SpawnSwords) // 플레이어 위치에 소환
            {
                GameObject SpawnSwordsSkill = Instantiate(playerSkills[(int)ESkillType.SpawnSwords],
                       player.transform.position,
                       player.transform.rotation);


                ParticleSystem[] particleSystems = SpawnSwordsSkill.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(SpawnSwordsSkill, 2f);

            }

        }

    }

    public void AttachParticle(ESkillType skillType, ESkillParticleType particleType)
    {
        if (particleType == ESkillParticleType.Attach)
        {
            // 찌르는 스킬
            if (skillType == ESkillType.Thrust)
            {
                GameObject ThrustSkill = Instantiate(playerSkills[(int)ESkillType.Thrust], 
                            player.skillSpawnPos[(int)ESkillType.Thrust].position,
                            player.skillSpawnPos[(int)ESkillType.Thrust].rotation, 
                            player.transform); 

                ParticleSystem[] particleSystems = ThrustSkill.GetComponentsInChildren<ParticleSystem>(); 

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(ThrustSkill, 1f);
            }

            else if (skillType == ESkillType.Shield)
            {
                GameObject ShieldSkill = Instantiate(playerSkills[(int)ESkillType.Shield],
                                player.skillSpawnPos[(int)ESkillType.Shield].position,
                                player.skillSpawnPos[(int)ESkillType.Shield].rotation,
                                player.transform);

                ParticleSystem[] particleSystems = ShieldSkill.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

            }
           
        }

    }

    public void AcquireParticle(GameObject dropSkill, ESkillGrade skillGrade)
    {

        if(skillGrade == ESkillGrade.Normal) 
        {
            GameObject acquireScroll = Instantiate(scrollParticle[(int)ESkillGrade.Normal], dropSkill.transform.position, Quaternion.identity);

            Destroy(acquireScroll, 1.5f);
        }
        
        else if(skillGrade == ESkillGrade.Epic)
        {
            GameObject acquireScroll = Instantiate(scrollParticle[(int)ESkillGrade.Epic], dropSkill.transform.position, Quaternion.identity);

            Destroy(acquireScroll, 1.5f);
        }
        
        else
        {
            GameObject acquireScroll = Instantiate(scrollParticle[(int)ESkillGrade.Unique], dropSkill.transform.position, Quaternion.identity);

            Destroy(acquireScroll, 1.5f);
        }
       
    }
}
