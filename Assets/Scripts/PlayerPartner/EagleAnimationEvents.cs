using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAnimationEvents : MonoBehaviour
{
    public void E_Skill()
    {
        GetComponent<Eagle>()?.EagleSkill();
    }

    public void E_SkillEnd()
    {
        GetComponent<Eagle>()?.EagleSkillEnd();
    }

    public void E_Leave()
    {
        GetComponent<Eagle>()?.EagleLeave();
    }
}
