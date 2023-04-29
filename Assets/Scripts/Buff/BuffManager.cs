using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    public Player player;

    public GameObject[] BuffAuras;

    public float strengthBuffTime;
    public float strengthBuffTimeMax;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    
        else
        {
            Destroy(this.gameObject);
        }

        strengthBuffTime = 0;
    }

    public void Update()
    {
        if(player.isStrengthBuff) // 플레이어의 힘이 강화된 상태라면
        {
            strengthBuffTime += Time.deltaTime;

            if(strengthBuffTime > strengthBuffTimeMax) // 만약 시간이 다 지났다면
            {
                strengthBuffTime = 0;
                player.isStrengthBuff = false;
                player.Strength = (int)(player.Strength * 0.5f);
                GameObject strengthAura = GameObject.Find("StrengthAura(Clone)"); // 일단 임시로 이렇게 제거하도록 하였다.
                Destroy(strengthAura);
            }
        }
      
    }
    // 현재 체력은 25만큼, 힘은 2배만큼 증가. 힘은 추후에 먹으면 시간 늘리는 방식으로 수정할 예정
    public void Buff(EBuffType buffType, GameObject gameObject, float buffTimeMax = 0)
    {
        if (buffType == EBuffType.HealthBuff) // 체력 버프인 경우
        {
            if (gameObject.CompareTag("Player")) // 받는 대상이 플레이어인 경우
            {
                Player player = gameObject.GetComponent<Player>();

                player.CurHealth += 25; // 나중에는 여기를 퍼센트로 올려주자

                if (player.CurHealth > player.MaxHealth) // 최대치 체력을 넘어가면 최대체력만큼 채워준다.
                {
                    player.CurHealth = player.MaxHealth;
                }
            }

            GameObject HealthBuff = Instantiate(BuffAuras[(int)EBuffType.HealthBuff],
                             gameObject.transform.position, gameObject.transform.rotation,
                             gameObject.transform);

            ParticleSystem[] particleSystems = HealthBuff.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particle in particleSystems)
            {
                particle.Play(); // 각 위치에 맞게 
            }

            Destroy(HealthBuff, 1f);
        }

        else if(buffType == EBuffType.StrengthBuff) 
        {
            if (gameObject.CompareTag("Player")) // 받는 대상이 플레이어인 경우
            {
                Player player = gameObject.GetComponent<Player>();

                if(!player.isStrengthBuff) // 힘 버프를 받고 있지 않은 경우에만 힘을 2배로 해준다.
                {
                    player.Strength *= 2;

                    player.isStrengthBuff = true;

                    strengthBuffTimeMax = buffTimeMax;
                }

                GameObject StrengthBuff = Instantiate(BuffAuras[(int)EBuffType.StrengthBuff],
                             gameObject.transform.position, gameObject.transform.rotation,
                             gameObject.transform);

                ParticleSystem[] particleSystems = StrengthBuff.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

            }


        }
    }

    // 같은 버프를 먹었을 때 버프 리셋
    public void SlotBuffReset(EBuffType buffType) 
    {
        if(buffType == EBuffType.StrengthBuff) 
        {
            strengthBuffTime = 0f;
        }    
    }

}
