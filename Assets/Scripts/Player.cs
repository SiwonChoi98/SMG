using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditorInternal;
using UnityEngine;


// 플레이어의 SkillType들, 일단 일반 공격은 0, 1, 2 로 고정, 한번에 딱 하나의 스킬만 사용 가능하다. 만약 유지형 스킬이 있다면, 그건 이쪽이 아닌 다른 쪽에서 처리해야 할듯
public enum EPlayerSkillType : int
{
    NormalAttack1,
    NormalAttack2, 
    NormalAttack3,
    SlotSkill,
    None
}

public class Player : MonoBehaviour, IDamageable
{
    [Header("조이스틱")]
    [SerializeField] private VariableJoystick joy;
    [Header("체력")]
    [SerializeField] private int curHealth; // 현재 체력
    [SerializeField] private int maxHealth; // 최대 체력
    [SerializeField] private int strength; // 플레이어의 공격력, 추후에 이 수치를 각 스킬들에게 전달할 예정
    [SerializeField] private float speed; // 플레이어의 이동속도
    public int CurHealth { get => curHealth; set => curHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Strength { get => strength; set => strength = value; }
    public float Speed { get => speed; set => speed = value; }
    //--------------------------------------------------------------------

    private Camera cameraMain;
    private Animator anim;
    private Rigidbody rigid;

    private float hAxis;
    private float vAxis;
    private bool dodge; // 회피키
    private bool attackKey; // 기본 공격 키

    private bool isDodging; // 회피 중인가?
    private bool isAttacking; // 일반 공격 하는 중인가?
    private bool isCasting; // 스킬을 캐스팅하고 있는가?

    private bool isAttackingMove; // 공격할 때 이동시작
    private bool isDodgeReady; // 회피 쿨타임이 다 초기화되서 다시 사용할 수 있는가?
    private float dodgeCoolTime; // 회피 쿨타임
    private float dodgeCoolTimeMax; // 회피 쿨타임 최대값
    private Vector3 dodgeVec;
    private Vector3 moveVec;

    

    // 임시적으로 확인하기 위해서 public으로 해두었다.
    public List<BaseSkill> playerSkills = new List<BaseSkill>(); // 가능한 공격 및 스킬을 담은 리스트, 이제는 오직 기본 공격을 위한 리스트로 변경해도 되지만, slotskill도 여기에 넣어버리자.
    
    public List<Transform> skillSpawnPos = new List<Transform>(); // 스킬들을 스폰할 위치를 담은 리스트

    public bool isShield; // 쉴드 상태인가?
    public int ShieldCount; // 쉴드 스킬 시 생성하는 보호막 개수

    public bool isStrengthBuff; // 힘 버프를 받고 있는가?
    public bool isSpeedBuff; // 스피드 버프를 받고 있는가?

    private EPlayerSkillType playerCurrentSkill; // 플레이어가 현재 수행하고 있는 스킬 타입

    public bool IsPlayerCanUseSkill => PlayerStateCheck(); // 플레이어가 현재 스킬을 사용할 수 있는지 슬롯에서 누를 때 전달해주는 bool값
    #region Unity Methods

    private void Awake()
    {
        cameraMain = Camera.main;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
       
    }

    private void Start()
    {

        Application.targetFrameRate = 60; // 이 부분 수정 예정

        dodgeCoolTimeMax = 3.0f;
        ShieldCount = 0;
        speed = 5f;
        maxHealth = 100;
        curHealth = maxHealth; // 처음에 피를 100으로 채워준다.
        strength = 10;
        isDodgeReady = true;
        dodgeCoolTime = dodgeCoolTimeMax;
    }

    private void Update()
    {   
        //AttackKey();
        //JoystickMove();
        //UseSlotSkill();
    }

    private void FixedUpdate() // 움직임 및 rigid를 사용하는 부분은 여기에 구현
    {
        GetInput();
        Move();
        AttackingMove(); // 일반 공격하거나 스킬을 캐스팅할 때 살짝씩 이동
        Turn();
        Dodge();
    }

    // 플레이어의 입력을 받아온다.
    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //ad
        vAxis = Input.GetAxisRaw("Vertical"); //ws
        dodge = Input.GetButtonDown("Jump"); // space
        //attackKey = Input.GetButtonDown("Fire1"); //마우스 왼쪽
        //skillKey_1 = Input.GetButtonDown("Skill1"); // 1번
        //skillKey_2 = Input.GetButtonDown("Skill2"); // 2번
        //skillKey_3 = Input.GetButtonDown("Skill3"); // 3번
        //skillKey_4 = Input.GetButtonDown("Skill4"); // 4번
    }

    #endregion Unity Methods

    #region Helper Methods

    // 플레이어가 현재 수행하고 있는 AttackBehavior가 뭔지 알거나 바꿔줘야 하므로, 다음과 같이 세팅
    public EPlayerSkillType GetPlayerSkillType() 
    {
        return playerCurrentSkill;
    }

    public void SetPlayerSkillType(EPlayerSkillType skillType)
    {
        playerCurrentSkill = skillType;
    }

    #endregion Helper Methods;

    #region Move Methods
    private void JoystickMove()
    {
        float x = joy.Horizontal;
        float y = joy.Vertical;
        moveVec = new Vector3(x, 0, y).normalized;

        if (isDodging)
            moveVec = dodgeVec;

        if (!isAttacking && !isCasting) // 공격 중이 아닌 경우 && 스킬 캐스팅 중이 아닌 경우
        {
            rigid.MovePosition(transform.position + moveVec * speed * Time.deltaTime);

            anim.SetBool("IsRun", moveVec != Vector3.zero);
        }
        //turn
        if (moveVec != Vector3.zero && !isAttacking && !isCasting && !isDodging) //가만이 있을때는 회전 불가 && 공격 중이 아닐 경우 && casting중이 아닐 경우  && 회피 상태가 아닌 경우
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 45 * Time.deltaTime);
            //rigid.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 45 * Time.deltaTime));
        }

    } //조이스틱 이동
    private void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        if (isDodging)
            moveVec = dodgeVec;

        if (!isAttacking && !isCasting) // 공격 중이 아닌 경우 && 스킬 캐스팅 중이 아닌 경우
        {
            rigid.MovePosition(transform.position + moveVec * speed * Time.deltaTime);
            
            if(isSpeedBuff) // 만약 스피드 버프가 켜져있는 상황이라면 다른 모션이 나오도록 한다.
            {
                anim.SetBool("IsRun", false);
                anim.SetBool("IsSprint", moveVec != Vector3.zero);
            }
            else
            {
                anim.SetBool("IsSprint", false);
                anim.SetBool("IsRun", moveVec != Vector3.zero);
            }
           
        }
    
    }

    private void Turn()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (moveVec != Vector3.zero && !isAttacking && !isCasting && !isDodging) //가만이 있을때는 회전 불가 && 공격 중이 아닐 경우 && casting중이 아닐 경우  && 회피 상태가 아닌 경우
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 45 * Time.deltaTime);
            //rigid.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 45 * Time.deltaTime));
        } 
            
    }

    private void Dodge()
    {
        dodgeCoolTime += Time.deltaTime;

        isDodgeReady = dodgeCoolTime >= dodgeCoolTimeMax;

        if (dodge && moveVec != Vector3.zero 
            && !isAttacking &&!isCasting && isDodgeReady) // 회피 키를 눌렀을 경우 && 움직임이 있는 경우 && 공격 상태가 아닌 경우 && 스킬 시전도 아닌 경우 && 닷지 쿨타임이 충족하는 경우
        {

            dodgeCoolTime = 0f;
            dodgeVec = transform.forward;
            speed = 10f;
            anim.SetTrigger("DoDodge");
            isDodging = true;

            Invoke("DodgeOut", 0.6f);
        }
        else if (dodge && moveVec != Vector3.zero 
            && isAttacking && !isCasting && isDodgeReady) // 회피 키를 눌렀을 경우 && 움직임이 있는 경우 && 공격 상태인 경우 && 스킬 시전도 아닌 경우&& 닷지 쿨타임이 충족하는 경우
        {
            // 하던 공격을 캔슬해야 하므로 취소해준다.
            PlayerAttackEnd();

            // 재생하고 있던 Particle System도 꺼줘야 한다.

            
            for (int i = 0; i < 3; i++)
            {
                playerSkills[i].ExitParticleSystem();
            }

            isDodging = true;
            dodgeCoolTime = 0f;
            dodgeVec = moveVec;
            transform.rotation = Quaternion.LookRotation(-moveVec).normalized;
            speed = 10f;
            anim.SetTrigger("DoCombatDodge");


            Invoke("CombatDodgeOut", 0.4f);
        }
    }

    private void DodgeOut()
    {
        if (isSpeedBuff)
        {
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }    
        isDodging = false;

        // Dodge에서 PlayerAttackEnd()를 해줬지만, 애니메이션 Transition 탈출할 때 AttackEnable을 거쳐서 나오면 다시 AttackEnable이 True가 된다.
        // 그러므로 Transition Duration을 0으로 해줘야 한다.
        //PlayerAttackEnd();
    }

    private void CombatDodgeOut()
    {
        if (isSpeedBuff)
        {
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }

        isDodging = false;
    }

    private void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    private void AttackingMove()
    {
        if (isAttacking || isCasting) // 공격하거나 캐스팅의 경우, 공격할 때는 playerSkills의 정보대로, isCasting일 때는 sloy의 정보대로로 바꿔야 한다.
        {
            if (isAttackingMove) // 만약 공격 행동을 실행하여 살짝 이동해야 한다면, 5f는 원래 스피드값
            {
                rigid.MovePosition(transform.position + 
                    transform.forward * 5f * Time.deltaTime * playerSkills[(int)playerCurrentSkill].attackForce);
            }
        }   
    }

    

    #endregion Move Methods

    #region NormalAttack Methods
    public void AttackKey()
    {
        //if (attackKey) // 공격키를 눌렀을 경우 
        //{
            if(!isAttacking &&!isDodging && !isCasting) // 공격 상태가 아닌 경우 && 회피 상태도 아닌 경우 && 스킬 시전 상태도 아닌 경우
            {
                isAttacking = true;
                anim.SetTrigger("DoAttack"); // 애니메이터에서 Attack 서브스테이트 머신으로 들어가게 만들고
                anim.SetBool("AttackEnd", false); // AttackEnd를 다시 False로 해준다.
                SoundManager.instance.SfxPlaySound(1);
                playerCurrentSkill = 
                    EPlayerSkillType.NormalAttack1; // 현재 공격 상태를 기본 공격 1로 해준다.

                playerSkills[(int)EPlayerSkillType.NormalAttack1].ExcuteParticleSystem(); // 첫번째 공격의 파티클을 재생시켜준다.
            }

            // bool을 변수로 뺐더니 오류 발생

            else if (isAttacking && !isDodging && !isCasting && anim.GetBool("AttackEnable"))  // 공격 상태인 경우 && 회피 중이지 않은 경우  && 스킬 시전 상태도 아닌 경우 그리고 다음 공격 애니메이션이 가능한 경우 
            {
                int attackComboNum = anim.GetInteger("AttackCombo");

                switch (attackComboNum) // 콤보가 1이면 2로, 2이면 3로, 3이면 다시 1으로 가게 해준다.
                {
                    case 1:
                        {
                            anim.SetInteger("AttackCombo", 2);
                            SoundManager.instance.SfxPlaySound(2);
                            playerCurrentSkill = 
                                EPlayerSkillType.NormalAttack2; // 현재 공격 상태를 기본 공격 2로 해준다.

                            playerSkills[(int)EPlayerSkillType.NormalAttack2].ExcuteParticleSystem();
                            break;
                        }
                    case 2:
                        {
                            anim.SetInteger("AttackCombo", 3);
                            SoundManager.instance.SfxPlaySound(3, 0.6f);
                            playerCurrentSkill = 
                                EPlayerSkillType.NormalAttack3; // 현재 공격 상태를 기본 공격 3으로 해준다.

                            playerSkills[(int)EPlayerSkillType.NormalAttack3].ExcuteParticleSystem();     
                            break;
                        }
                    case 3:
                        {
                            anim.SetInteger("AttackCombo", 1);
                            SoundManager.instance.SfxPlaySound(1);
                            playerCurrentSkill = 
                                EPlayerSkillType.NormalAttack1; // 현재 공격 상태를 기본 공격 1로 해준다.

                            playerSkills[(int)EPlayerSkillType.NormalAttack1].ExcuteParticleSystem();
                            break;
                        }
                }

            }

        //}

       
    }

    // 애니메이션 이벤트들을 public으로 해야 외부에서 호출 가능하다.
    // 그리고 애니메이션 이벤트 함수와 똑같은 메소드 이름 쓰면 동시에 실행되므로 다르게 해야 한다.

    // 공격 애니메이션의 시작부터 호출
    public void PlayerAttackStart()
    {
        anim.SetBool("AttackEnable", false);

        isAttackingMove = true;
    }

    // 공격 애니메이션 중 타격 부분을 실행하면 호출
    public void PlayerAttack()
    {
        
        int attackComboNum = anim.GetInteger("AttackCombo");
        
        switch (attackComboNum)
        { 
            case 1:
                {
                    playerSkills[(int)EPlayerSkillType.NormalAttack1].ExcuteAttack(); 
                    break; 
                }

            case 2:
                {
                    playerSkills[(int)EPlayerSkillType.NormalAttack2].ExcuteAttack();
                    break;
                }

            case 3:
                {
                    playerSkills[(int)EPlayerSkillType.NormalAttack3].ExcuteAttack();
                    break;
                }
        }

    }


    // 공격 애니메이션 중 다음 공격이 가능한 시점부터 호출 
    public void PlayerAttackEnable()
    {
        anim.SetBool("AttackEnable", true);

        isAttackingMove = false;
    }

    // 공격 애니메이션 중 거의 끝나갈 때 쯤 호출
    public void PlayerAttackEnd()
    {
        anim.SetBool("AttackEnable", false);
        anim.SetInteger("AttackCombo", 1);
        anim.SetBool("AttackEnd", true);
        isAttacking = false;

    }

    #endregion Attack Methods

    #region Skill Methods

    // 이 부분을 UseSlotSkill(BaseSkill skill)로 해서, 슬롯의 스킬을 사용하는 식으로 가자.
    // 스킬을 사용할 때, 슬롯에서 BaseSkill을 전달해주고, 저기서 검사까지 마치고 왔으므로, 해당 스킬을 틀어주면 된다. 해당 스킬에 포함되어야 하는 정보는 더 있는데,
    // 우선 BaseSkill은 ExcuteParticleSystem은 가능하다. 또한 ExcuteAttack도 가능하다. BaseSkill을 어딘가에 저장하면서 ex) SlotSkills 넣어줌으로써 가능하다.
    public void UseSlotSkill(BaseSkill _skill = null)                    
    {
        if(playerSkills[(int)EPlayerSkillType.SlotSkill] == null) // 만약 현재 슬롯 스킬이 비어있는 상태여야 넣어준다.
        {
            playerSkills[(int)EPlayerSkillType.SlotSkill] = _skill;
            isCasting = true;
            anim.SetTrigger("DoSkill");
            anim.SetInteger("SkillNumber", (int)_skill.mSkillType); // 스킬의 mSkill 번호대로 스킬 애니메이션을 실행해준다.

          

            anim.SetBool("SkillEnd", false); // SkillEnd를 다시 False로 해준다.
            playerCurrentSkill = EPlayerSkillType.SlotSkill;

            _skill.ExcuteParticleSystem();
        }
    }

    public void PlayerSkillStart()
    {
        isAttackingMove = true;
    }

    // 공격 애니메이션 중 타격 부분을 실행하면 호출
    public void PlayerSkill()
    {
        if(playerSkills[(int)EPlayerSkillType.SlotSkill] != null) // 스킬이 제대로 슬롯 스킬칸에 들어간 경우에만,
        {
            playerSkills[(int)EPlayerSkillType.SlotSkill].ExcuteAttack();
        }
       
    }


    // 공격 애니메이션 중 다음 공격이 가능한 시점부터 호출 
    public void PlayerSkillEnable()
    {
        isAttackingMove = false;
    }

    // 공격 애니메이션 중 거의 끝나갈 때 쯤 호출
    public void PlayerSkillEnd()
    {

        anim.SetInteger("SkillNumber", -1);
        anim.SetBool("SkillEnd", true);

        if (playerSkills[(int)EPlayerSkillType.SlotSkill] != null) // 스킬이 제대로 슬롯 스킬칸에 들어간 경우에만,
        {
            playerSkills[(int)EPlayerSkillType.SlotSkill] = null;
        }

        isCasting = false;
    }

    #endregion Skill Methods

    #region IDamageable Methods
    public bool IsAlive => curHealth > 0;

    public void TakeDamage(int damage, GameObject hittEffectPrefab) // 맞았을 경우 플레이어가 피격당하는 위치를 어떻게 할지
    {
        if (!IsAlive)
        {
            return;
        }

        if (ShieldCount > 0)
        {
            ShieldCount -= 1;
            GameManager.instance.playerShieldImage.fillAmount = (float)1 / 3;
            if (ShieldCount == 0)
            {
                GameManager.instance.playerShieldImage.gameObject.SetActive(false);
            }
            return;
        }

        curHealth -= damage;

        Debug.Log("Damage : " + damage);

        if (IsAlive)
        {

        }
        else
        {
            anim.SetTrigger("Death");
        }

    }
    public void KnockBack(float knockBackForce, Vector3 dir)
    {

        rigid.AddForce(dir.normalized * knockBackForce, ForceMode.Impulse);

    }
    #endregion IDamageable Methods

    private bool PlayerStateCheck() // 여기 부분에 나중에 isDamage 같은 플레이어 피격 상태도 넣어야 할듯
    {
        return (!isAttacking && !isDodging && !isCasting);
    }
}

