using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Skill_Shield : BaseSkill
{


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
 
    }


    public override void ExcuteParticleSystem()
    {
        Player _player = SkillManager.instance.player;

        _player.ShieldCount = 3; //+= 3이 아닌 =으로 수정
        GameManager.instance.playerShieldImage.fillAmount = 1;
        GameManager.instance.playerShieldImage.gameObject.SetActive(true);
        if (_player.isShield == false) // 쉴드가 없는 경우에만 파티클 생성
        {
            _player.isShield = true;

            SkillManager.instance.AttachParticle(mSkillType, mParticleType);
        }
       
        SoundManager.instance.SfxPlaySound(6);
    }

    public override void ExitParticleSystem()
    {

    }
}
