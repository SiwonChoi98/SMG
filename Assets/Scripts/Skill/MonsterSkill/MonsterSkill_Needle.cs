using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill_Needle : BaseSkill
{
    [SerializeField]
    GameObject needle;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Vector3 vec = transform.position;

        if (startPoint)
        {
            vec = startPoint.position;
        }

        GameObject go = Instantiate(needle, startPoint.position, startPoint.rotation);

        go.GetComponent<Needle>().SetDamage((int)(damage));
        go.GetComponent<Needle>().SetTarget(targetMask);

    }

    public override void ExcuteParticleSystem()
    {       
    }

    public override void ExitParticleSystem()
    {


    }
}
