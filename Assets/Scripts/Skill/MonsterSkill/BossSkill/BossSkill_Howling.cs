using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill_Howling : BaseSkill
{
    [SerializeField]
    GameObject summonWolf;

    [SerializeField]
    GameObject[] spawnPos;

    [SerializeField]
    GameObject SummonParticle;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        ExcuteParticleSystem();

        StartCoroutine(SpawnWolf());


    }


    public override void ExcuteParticleSystem()
    {
        
    }

    public override void ExitParticleSystem()
    {


    }

    IEnumerator SpawnWolf()
    {
 
        for(int i = 0; i < spawnPos.Length; i++) 
        {
            ParticleSystem[] spawnParticle = spawnPos[i].GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particle in spawnParticle)
            {
                particle.Play();
            }
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < spawnPos.Length; i++) 
        {
            GameObject wolf = Instantiate(summonWolf, spawnPos[i].transform.position, spawnPos[i].transform.rotation);

        } 

        yield return null;
    }
}
