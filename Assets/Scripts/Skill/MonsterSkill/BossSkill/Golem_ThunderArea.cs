using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_ThunderArea : MonoBehaviour // GroundAttackSkill은 바닥 찍는 공격과 번개 스킬 데미지가 따로 있어서 이렇게 처리하였다.
{
    [SerializeField]
    int damage;

    [SerializeField]
    GameObject hitFx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 범위 안에 들어온 콜라이더가 플레이어라면
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, hitFx, 1);
        }
    }
}
