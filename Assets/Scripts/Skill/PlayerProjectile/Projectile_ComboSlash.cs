using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_ComboSlash : MonoBehaviour
{
    public LayerMask TargetMask;

    [SerializeField]
    float speed = 1f;

    int damage = 1;

    [SerializeField]
    float DestroyTime = 1f;

    [SerializeField]
    GameObject hitFx;


    void Update()
    {

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        Destroy(this.gameObject, DestroyTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (1 << collision.transform.gameObject.layer == TargetMask)
        {
            float randDamage = Random.Range(damage, damage + damage * 0.2f);
            //Debug.Log("collision.gameObject.layer : " + collision.gameObject.layer);
            collision.gameObject.GetComponent<Monster>()?.SetHitBySkill(true);
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)randDamage, hitFx);
            collision.gameObject.GetComponent<Monster>()?.KnockBack(6f);  // 임시

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
}
