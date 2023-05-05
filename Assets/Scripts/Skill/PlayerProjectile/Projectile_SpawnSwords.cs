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
            
            if(currentSpeed < speed)
            {
                currentSpeed += speed * Time.deltaTime;
            }
            transform.position += transform.up * currentSpeed * Time.deltaTime;

            Vector3 t_dir = (target.position + Vector3.up * 1f - transform.position).normalized; // 몬스터의 transform이 다 바닥에 있어서 살짝 위로 날아가야 한다.
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.3f); // 값이 클수록 적에게 빨리 회전해서 날아간다.
        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (1 << collision.transform.gameObject.layer == TargetMask)
        {
            //Debug.Log("collision.gameObject.layer : " + collision.gameObject.layer);
            collision.gameObject.GetComponent<Monster>()?.SetHitBySkill(true);
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, hitFx);
            collision.gameObject.GetComponent<Monster>()?.KnockBack(6f);  // 임시
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

    void SearchEnemy()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, 100f, TargetMask);

        if(t_cols.Length > 0) 
        {
            target = t_cols[Random.Range(0, t_cols.Length)].transform;
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
