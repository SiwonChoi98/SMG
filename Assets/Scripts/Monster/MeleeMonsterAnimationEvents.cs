using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonsterAnimationEvents : MonoBehaviour
{

    
    // 만약 몬스터도 때릴 때 자연스럽게 이동하게 할려면 플레이어처럼 AttackingMove 만들고 AttackStart와 AttackEnable 만들어야 한다.
    public void M_SKill()
    {
        GetComponent<DefaultMeleeMonster>()?.MonsterSKill();
    }

    public void M_SkillEnd()
    {
        GetComponent<DefaultMeleeMonster>()?.MonsterSkillEnd();

    }

}
