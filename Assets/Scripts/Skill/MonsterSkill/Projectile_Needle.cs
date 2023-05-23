using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Needle : MonoBehaviour
{
    public LayerMask TargetMask;

    [SerializeField]
    float speed = 1f;

    int damage = 1;

    [SerializeField]
    float DestroyTime = 1f;

    [SerializeField]
    int projectileLevel;

    [SerializeField]
    GameObject hitFx;


    void Update()
    {

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        Destroy(this.gameObject, DestroyTime); // 맞지 않더라도 일정 시간 지나면 제거
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (1 << collision.transform.gameObject.layer == TargetMask)
        {
            //Debug.Log("collision.gameObject.layer : " + collision.gameObject.layer);
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, hitFx, projectileLevel);
            Destroy(this.gameObject); // 맞으면 바로 제거
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

    public void SetHitLevel(int i)
    {
        projectileLevel = i;
    }
}
