using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Strength : BaseBuff
{
    public override void ExcuteBuff(GameObject gameObject = null)
    {
        if (gameObject != null) // ExcuteBuff에서 버프 대상을 정해준 경우에만 실행
        {
            buffTarget = gameObject; // Buff할 대상을 지정해준다.

            BuffManager.instance.Buff(mBuffType, buffTarget, buffTimeMax);

        }

    }
}
