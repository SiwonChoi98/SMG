using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Barrier : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")) // 범위 안에 들어온 콜라이더가 플레이어라면
        {
            Player player = other.GetComponent<Player>();
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            player.KnockBack(4f, dir); // 임시로 4만큼 밀어준다.
        }
    }
}
