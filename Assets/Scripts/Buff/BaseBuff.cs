using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBuffType : int // 버프의 타입, 디버프도 포함
{
   HealthBuff,
   LargeHealthBuff,
   StrengthBuff,
   SpeedBuff
}

public abstract class BaseBuff : MonoBehaviour
{
    #region Variables

    public float buffTime;

    public float buffTimeMax;

    public EBuffType mBuffType;

    public GameObject buffTarget;

    #endregion Variables
    protected virtual void Start()
    {
        buffTime = 0;
    }

    protected virtual void Update()
    {
    }

    // Start is called before the first frame update
    public abstract void ExcuteBuff(GameObject gameObject = null); // 버프 실행 -> 버프 적용 대상을 정해주고, 각 시간은 buffTime 맞게 적용

    // 버프는 공격과 다르게 얻는 즉시 이펙트가 생겨나면 된다. 따라서 파티클을 따로 관리할 필요가 없다.

}
