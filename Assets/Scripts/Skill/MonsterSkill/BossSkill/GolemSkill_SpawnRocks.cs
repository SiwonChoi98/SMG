using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSkill_SpawnRocks : BaseSkill
{
    [SerializeField]
    GameObject[] rockObjects;

    [SerializeField]
    GameObject[] spawnPos;


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        for(int i = 0; i < rockObjects.Length; i++) 
        {
            GameObject go = Instantiate(rockObjects[i],
            spawnPos[i].transform.position,
            spawnPos[i].transform.rotation);

            go.GetComponent<Projectile_Needle>().SetDamage((int)(damage));
            go.GetComponent<Projectile_Needle>().SetTarget(targetMask);
        }

    }


    public override void ExcuteParticleSystem()
    {

    }

    public override void ExitParticleSystem()
    {


    }
}
