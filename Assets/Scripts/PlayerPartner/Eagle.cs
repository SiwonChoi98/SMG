using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Eagle : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody rigid;
    public Animator anim;

    [SerializeField] private float _attackRange; //공격 거리
    [SerializeField] private float _attackTime = 0f; //공격 쿨타임
    [SerializeField] private Monster target; // 몬스터를 타겟으로 받아온다.
    [SerializeField] private float coolTime = 0f; // 동료가 나와있는 시간 체크

    [SerializeField] private float coolTimeMax = 15f; // 나와있는 최대 시간
    public LayerMask targetMask;
    public bool isAttackRange = false; //공격할수 거리인지 체크 변수
    public bool isAttack = false; //공격 쿨타임 지났는지 체크 변수, 이 부분은 보스에게는 공격 쿨타임이 여러개 있거나
    public bool isMonster = false;

    private float _initialAttackTime; //공격 쿨타임 초기화
    public BaseSkill eagleSkill; // 이글 공격

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        _initialAttackTime = 3f;
    }

    // Update is called once per frame
    private void Update()
    {
      
        if(target != null && target.IsAlive) // 타겟이 있다면 && 그리고 살아있다면
        {
            Attack();
        }
        AttackCoolTime();
        CheckCoolTime();
    }

    public void AttackDistanceCheck()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _attackRange, targetMask);

        if (cols.Length > 0) // 몬스터가 하나라도 있으면
        {
            for (int i = 0; i < cols.Length; i++)
            {
                
                target = cols[i].gameObject.GetComponent<Monster>();
                
                if (target.IsAlive)
                {
                    break;
                }
               
            }
        }
    }

    private void Attack()
    {
        if(isAttack && target.IsAlive) // 공격 가능하고 타겟이 활성화되어있다면
        {
            anim.SetBool("SkillEnd", false);
            anim?.SetTrigger("doAttack");
            transform.LookAt(target.transform.position);
            Debug.Log("Attack 성공");
            isAttack = false;
        }
    }

    private void AttackCoolTime()
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

    private void CheckCoolTime()
    {
        coolTime += Time.deltaTime;

        if(coolTime >= coolTimeMax && anim.GetBool("SkillEnd")) 
        {
            coolTime = 0; // 다시 못들어오게 해준다.
            _attackTime = 10f; // 떠나는 중에는 공격도 초기화해 공격도 못하게 해준다.
            anim.SetTrigger("doLeave");
        }
    }


    public void EagleSkill()
    {
        eagleSkill.ExcuteAttack(target.gameObject);
    }

    public void EagleSkillEnd()
    {
        anim.SetBool("SkillEnd", true);
    }

    public void EagleLeave()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            AttackDistanceCheck();

            //if (target == null) // 소환되자마자는 타겟이 없으므로 할당시켜준다.
            //{
            //    Debug.Log("Monster 첫 탐지");

            //    target = other.gameObject.GetComponent<Monster>();
            //}
            //else if (target != null && !target.enabled) // 타겟이 비활성화되면 다시 타겟을 찾아준다.
            //{
            //    AttackDistanceCheck();
            //}
            //AttackDistanceCheak();
        }
    }
}
