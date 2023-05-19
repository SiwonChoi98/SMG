using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_SpawnSwords : MonoBehaviour
{
    public LayerMask TargetMask;

    Rigidbody rigid = null;
    
    Transform target;
    
    [SerializeField]
    float speed = 0f;

    float currentSpeed = 0f;

    int damage = 1;

    [SerializeField]
    GameObject hitFx;


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        
        StartCoroutine(LaunchDelay());
    }

    private void Update()
    {
        if(target != null) 
        {
            if(!target.GetComponent<Monster>().IsAlive) // 날아가다가 타겟이 사망하면 바로 제거
            {
                Destroy(gameObject);
            }

            if(currentSpeed < speed)
            {
                currentSpeed += speed * Time.deltaTime;
            }
            transform.position += transform.up * currentSpeed * Time.deltaTime;

            Vector3 t_dir = (target.position - transform.position).normalized; // 몬스터의 transform이 다 바닥에 있어서 살짝 위로 날아가야 한다.
            
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.7f); // 값이 클수록 적에게 빨리 회전해서 날아간다.
        }

        
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.transform.gameObject.layer == TargetMask)
        {
            //Debug.Log("collision.gameObject.layer : " + collision.gameObject.layer);
            float randDamage = Random.Range(damage, damage + damage * 0.2f);

            other.gameObject.GetComponent<Monster>()?.SetHitBySkill(true);
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)randDamage, hitFx);
            other.gameObject.GetComponent<Monster>()?.KnockBack(6f);  // 임시
            Destroy(gameObject);

        }
    }

    public void SetDamage(int i)
    {
        damage = i;
    }

    public void SetTarget(LayerMask layerMask) // 
    {
        TargetMask = layerMask;
    }

    public void SetHitEffect(GameObject hitEffect)
    {
        hitFx = hitEffect;
    }
    void SearchEnemy()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, 100f, TargetMask);

        if(t_cols.Length > 0) 
        {
            target = t_cols[Random.Range(0, t_cols.Length)].transform;
            this.GetComponent<BoxCollider>().isTrigger = true; // 칼들이 붙었다가 떨어지는 순간에 다시 트리거를 활성화 시켜준다.
        }
        else
        {
            Destroy(gameObject);
        }

    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => rigid.velocity.y < 0f); // 칼이 다시 아래로 떨어지기전까지 대기

        yield return new WaitForSeconds(0.1f);

        SearchEnemy();
        // 여기서 트레일을 추가하여도 된다.

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);

    }
}
