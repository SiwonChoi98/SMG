using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public enum EMonsterType : int
{
    Common, // 일반몹
    Elite, // 엘리트몹
    Boss // 보스몹
}

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] protected string _name;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _curHealth;
    [SerializeField] protected int _speed; //현재는 네비게이션안에 speed 써서 speed는 따로 정의가 필요없다. 추후에 바꿀예정
    // 이 부분에서 각각의 몬스터마다 speed를 받아올 수 있도록하자.

    [SerializeField] protected float _attackRange; // 기본적인 공격 거리, 보스를 제외한 몬스터들은 대부분 하나만을 가진다.
    [SerializeField] protected float _attackRange2; // 보스의 스킬 공격 거리, 근접 공격을 하다가 이 거리로 공격을 한다. 일반이나 엘리트는 기본적으로 0이다.
    [SerializeField] protected float _attackTime; //공격 쿨타임
    [SerializeField] public float _hitTime; // 피격 쿨타임
    public List<BaseSkill> monsterSkills = new List<BaseSkill>(); // 새로추가한 부분, 가능한 공격 및 스킬을 담은 리스트

    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int CurHealth { get => _curHealth; set => _curHealth = value; }

    public int Speed { get => _speed; set => _speed = value; }
    public Rigidbody rigid;
    public Animator anim;

    [SerializeField] public Transform target;

    protected StateMachine<Monster> stateMachine;

    public Transform projectilePoint; // 투사체가 나가는 방향
    
    [Header("상태 체크 변수")]

    public EMonsterType mType; // 몬스터 등급
    public bool isMove = false; //이동할수 있는지 체크 변수
    public bool isAttackRange = false; //공격할수 거리인지 체크 변수
    public bool isAttack = false; //공격 쿨타임 지났는지 체크 변수, 이 부분은 보스에게는 공격 쿨타임이 여러개 있거나
    public bool isHit = false; //공격받았는지 체크 변수
    public bool isHitEnd = true; // 공격 받고 있는 상태가 아닌지 체크 변수 
    public bool isHitbySkill = false; // 스킬에 의한 공격을 받았는지 체크 변수

    // 보스 전용 변수
    protected bool isAttackingMove = false; // 몬스터 애니메이션 시 움직이는 경우 체크 변수
    protected bool isAttackingTurn = false; // 몬스터 애니메이션 시 회전하는 경우 체크 변수
    protected float dir; // 몬스터 애니메이션 시 움직임의 방향 및 크기 


    private float _distance; //플레이어(타겟)과의 거리
    protected float _initialAttackTime; //공격 쿨타임 초기화
    public float _initialHitTime; // 피격 쿨타임 초기화
    public GameObject attackPrefab; //임시) 원거리 공격 오브젝트
    protected float _attackSpeed; //나가는 속도

    public Image healthImage;
    public GameObject dmgText;
    public Transform dmgTextPos;
    private string _dmgTextFolderName = "DamageText/dmgText";
    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
        InitStat();
    }

    //임시 능력치 셋팅, 자식 몬스터들이 구현하도록 함
    protected virtual void InitStat()
    {
    
    } 
    protected virtual void Start()
    {
        dmgTextPos = this.gameObject.transform;
        dmgText = (GameObject)Resources.Load(_dmgTextFolderName);

        stateMachine = new StateMachine<Monster>(this, new IdleState()); //현재 상태입력
        AddState();//여러 상태 삽입
    }
    protected void AddState()
    {
        stateMachine.AddState(new AttackState()); //모든 상태들 삽입
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new DeadState());
        stateMachine.AddState(new HitState());
    }
    protected virtual void Update()
    {
        AttackDistanceCheak();
        AttackCoolTime();
        HitCoolTime();
        stateMachine.Update(Time.deltaTime); //상태 계속 체크
    }



    // 새로 추가한 부분
    #region IDamageable Methods 
    public bool IsAlive => _curHealth > 0; // 이 부분을 뺄 수도 있음

    public void TakeDamage(int damage, GameObject hittEffectPrefab)
    {
        if (!IsAlive)
        {
            return;
        }

        _curHealth -= damage;
        DamageText(damage); //데미지 텍스트
        //SoundManager.instance.SfxPlaySound(0); //테스트
        if (mType == EMonsterType.Common) // 만약 일반 몬스터라면 바로 hit 처리
        {
            isHit = true; // 데미지 깎이면서 isHit을 true로
        }

        else if(mType == EMonsterType.Elite && isHitbySkill) // 만약 엘리트 몬스터이며 && 스킬에 맞았을 경우에만 hit 처리
        {
            isHit = true;
            //isHitbySkill = false;
        }
        // 보스는 hit 상태로 넘어갈 일이 없다.

        Debug.Log("Damage : " + damage);

        if(_curHealth <= 0) 
        {
            stateMachine.ChangeState<DeadState>();
            GameManager.instance.currentMonsterCount--;
        }

    }

    // 스킬에 의해 맞았는가?
    public void SetHitBySkill(bool isSKill) 
    {
        isHitbySkill = isSKill;
    }
    // y값을 빼줘야 정상적으로 밀린다.
    public void KnockBack(float knockBackForce)
    {
        if(mType == EMonsterType.Common) // 몬스터가 일반 몬스터면 무조건 밀린다.
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0;
            Debug.Log("Knockback : " + knockBackForce);
            rigid.AddForce(dir.normalized * -1 * knockBackForce, ForceMode.Impulse);
        }
        else if(mType == EMonsterType.Elite) // 몬스터가 엘리트면 스킬 공격에만 밀린다.
        {
            if(isHitbySkill)
            {
                Vector3 dir = target.position - transform.position;
                dir.y = 0;
                Debug.Log("attackForce : " + knockBackForce);
                rigid.AddForce(dir.normalized * -1 * knockBackForce, ForceMode.Impulse);
                isHitbySkill = false;
            }
        }
        // 보스면 밀리지 않는다.
       

        //this.GetComponent<NavMeshAgent>().enabled = true;
        //rigidbody.AddForce(transform.up, ForceMode2D.Impulse);
    }

    #endregion IDamageable Methods

    // 거리 체크해서 공격할 수 있는 상태만들기

    #region Attack Methods
    public bool AttackDistanceCheak() // 이제 이 부분을 AttackRange1 과 AttackRange2로 나눌 것이다. 보스는 이 공격을 주로 
    {
        Vector3 thisPos = transform.position; 
        Vector3 targetPos = target.position;

        thisPos.y = 0;
        targetPos.y = 0;

        _distance = Vector3.Distance(thisPos, targetPos); // 나중에 플레이어와 몬스터의 키를 뺴준다. 완료
        
        if (_distance < _attackRange) // 만약 플레이어가 공격 사거리1 내로 들어왔다면,
        {
            return isAttackRange = true;
        }
        else
        {
            return isAttackRange = false;
        }
    }

    // 공격 쿨타임
    public void AttackCoolTime()
    {
        if (!isAttack)
        {
            _attackTime -= Time.deltaTime;
            if (_attackTime < 0 && anim.GetBool("SkillEnd"))
            {
                _attackTime = _initialAttackTime;
                isAttack = true;
            }
        }
    }


    public void HitCoolTime()
    {
        if(!isHitEnd)
        {
            _hitTime -= Time.deltaTime;
            if (_hitTime < 0)
            {
                _hitTime = _initialHitTime;
                isHitEnd = true;
            }
        }
    }

    public virtual void Shoot() // 현재 Shoot은 공격 시작 시  Target을 향해 돌아보는 역할
    {

    }

    // 애니메이션 이벤트 호출 부분
    public void MonsterSKillStart(float degree) 
    {
        isAttackingMove = true;
        dir = degree;
        isAttackingTurn = true;
    }

    public void MonsterSKillEnable()
    {
        isAttackingMove = false;
        isAttackingTurn = false;
    }

    public virtual void MonsterSKill(int index) // 실제 공격이 나가거나 투사체가 나가야하는 타이밍, projectilePoint가 있는 경우 저기서 스킬이 나갈 것이다.
    {
    
    }

    public void MonsterSkillEnd() // 공격 애니메이션이 다 끝났는지 확인함
    {
        anim.SetBool("SkillEnd", true); // true로 만들어주면 attack motion 탈출
        
        if(mType == EMonsterType.Boss)
        {
            anim.SetInteger("SkillNumber", -1);
        }
    }

    public virtual void MonsterSkillCharge() // 애니메이션 중간에 기를 모으거나 투사체를 모음
    {

    }

    #endregion Attack Methods

    public void DamageText(int dmg)
    {
        GameObject dmgtext1 = Instantiate(dmgText, dmgTextPos.position + new Vector3(0,2,0), Quaternion.Euler(70, 0, 0)); //Quaternion.identity 원래 가지고있는 각도로 생성
        dmgtext1.GetComponentInChildren<Text>().text = dmg.ToString();//자식텍스트로 들어가서 //dmg는 int니까 string형태로 바꿔주기
        Destroy(dmgtext1, 0.7f);
    }
    private void LateUpdate()
    {
        //healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, (float)CurHealth /MaxHealth / 1 / 1, Time.deltaTime * 5); //체력
    }

}
