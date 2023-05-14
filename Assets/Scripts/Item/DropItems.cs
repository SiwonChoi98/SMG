using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemIndex
{

    Thrust,
    SpawnSwords,
    UpperSlash,
    Shield,

    GiantSword,
    ComboSlash,
    SpawnEagle,

    Baldo,
    GroundBreak,

    Heal,
    Speed,
    Strength
}

public class DropItems : MonoBehaviour
{
    public GameObject[] items;

    private EMonsterType mMonsterType;

    private void Awake()
    {
        mMonsterType = this.GetComponent<Monster>().mType;
    }

    public void DropItem()
    {   

        if(mMonsterType == EMonsterType.Common) // 일반 몬스터인 경우
        { 
            int randNum = Random.Range(0, 100);
            
            if(randNum >= 0 && randNum < 70) // 70 퍼센트의 확률로 스킬 생성
            {
                int normalSkillNum = Random.Range(0, 6); // 0부터 3까지 나오면 해당 스킬 생성, 4부터 5까지 나오면 해당 스킬 생성 안함 
                
                if (normalSkillNum >= 0 && normalSkillNum <= 3) 
                {
                    Instantiate(items[normalSkillNum], this.transform.position, this.transform.rotation);
                }

                else
                {
                    return;
                }
            }

            else
            { 
                int buffNum = Random.Range(9, 13); // 9부터 10까지는 버프 생성, 11부터 12까지는 생성 안함

                if(buffNum >= 9 && buffNum <= 10)
                {
                    Instantiate(items[buffNum], this.transform.position, this.transform.rotation);
                }

                else
                {
                    return;
                }
            }

        }

        else if(mMonsterType == EMonsterType.Elite)
        {
            int randNum = Random.Range(0, 100);

            if (randNum >= 0 && randNum < 70) // 70 퍼센트의 확률로 스킬 생성
            {
                int eliteSkillNum = Random.Range(4, 7); // 4부터 6까지 나오면 해당 스킬 생성
                
                Instantiate(items[eliteSkillNum], this.transform.position, this.transform.rotation);
            }

            else
            {
                int buffNum = Random.Range(9, 12); // 9부터 11까지는 버프 생성

                Instantiate(items[buffNum], this.transform.position, this.transform.rotation);
            }
        }

        else if (mMonsterType == EMonsterType.Boss)
        {
            int specialSkillNum = Random.Range(7, 9); // 7부터 8까지 나오면 해당 스킬 생성

            Instantiate(items[specialSkillNum], this.transform.position, this.transform.rotation);

        }
    }
}
