using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬이 플레이어에 붙어있어야 하는지, 아니면 소환되어야 하는지
public enum ESkillParticleType : int
{
    Attach,
    Spawn
}

// 플레이어가 사용하는 스킬 타입
public enum ESkillType : int 
{
    Thrust,
    GiantSword,
    GroundBreak,
    UpperSlash,
    Shield,
    Baldo,
    SpawnEagle,
    NormalAttack
}

public abstract class BaseSkill : MonoBehaviour 
{
    // 여기 Variables의 public도 진짜 필요한 것만 public으로 처리하자.
    #region Variables

    public ParticleSystem[] particleSystems; // 일반 공격에서 사용하는 파티클 시스템,

    public int damage; // 플레이어가 

    [SerializeField]
    public float attackForce; // 해당 공격 행동을 할 때 나아가는 방향에 곱해주는 정도.

    public ESkillType mSkillType;

    public ESkillParticleType mParticleType;

    [SerializeField]
    private float coolTime;

    [SerializeField]
    private float calcCoolTime = 0.0f;

    //[HideInInspector]

    public LayerMask targetMask;

    [SerializeField]
    public bool IsAvailable => calcCoolTime >= coolTime;

    public GameObject effectPrefab; // 타격했을 때 생기는 프리팹

    #endregion Variables


    protected virtual void Awake() 
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(); // 자식들로부터 ParticleSystem 들을 받아온다.
    }


    protected virtual void Start()
    {
        calcCoolTime = coolTime;
    }

    protected void Update()
    {
        if (calcCoolTime < coolTime) 
        {
            calcCoolTime += Time.deltaTime;
        }
    }


    // 공격할 때 때린 상대방과, 어디서 스킬을 발생시킬 것인지를 가져온다.
    // 플레이어의 검과 가깝고 붙어있는 스킬들은 자체적으로 조정하기 떄문에 필요없지만, 소한하는 형태의 스킬은 생성 위치를 전달해줘야 한다. 
    public abstract void ExcuteAttack(GameObject target = null, Transform startPoint = null); // 공격 실행, 애니메이션 이벤트 함수 Attack()에 호출
    public abstract void ExcuteParticleSystem(); // 파티클 발생, 애니메이션을 실행할 때 같이 호출
    public abstract void ExitParticleSystem(); // 파티클 제거, 애니메이션을 실행할 때 같이 호출
}
