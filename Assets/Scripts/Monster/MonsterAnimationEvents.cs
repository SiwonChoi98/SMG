using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEvents : MonoBehaviour
{

    
    // 만약 몬스터도 때릴 때 자연스럽게 이동하게 할려면 플레이어처럼 AttackingMove 만들고 AttackStart와 AttackEnable 만들어야 한다.

    public void M_SkillStart(float degree) // 얼마만큼 이동을 시킬건지
    {
        GetComponent<Monster>()?.MonsterSKillStart(degree);
    }

    public void M_SkillEnable()
    {
        GetComponent<Monster>()?.MonsterSKillEnable();
    }

    public void M_SKill(int index)
    {
        GetComponent<Monster>()?.MonsterSKill(index);
    }

    public void M_SkillEnd()
    {
        GetComponent<Monster>()?.MonsterSkillEnd();

    }


    public void M_SkillCharge() // 스킬 차지에 필요한 부분
    {
        GetComponent<Monster>()?.MonsterSkillCharge();
    }

}
