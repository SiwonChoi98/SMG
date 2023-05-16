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

    public void DropSkill()
    {   
        if(mMonsterType == EMonsterType.Common) // 일반 몬스터인 경우
        { 
            int randNum1 = Random.Range(0, 100);
            
            if(randNum1 >= 0 && randNum1 < 30) // 30 퍼센트의 확률로 스킬 생성
            {
                int randNum2 = Random.Range(0, 100);
                
                if( randNum2 >= 0 && randNum2 < 80) // 80퍼 : 일반 스킬 생성
                {
                    int normalSkillNum = Random.Range(0, 4); 

                    Instantiate(items[normalSkillNum], this.transform.position, this.transform.rotation);
                }
                else  // 20퍼 : 에픽 스킬 생성
                {
                    int  epicSkillNum = Random.Range(4, 7); 

                    Instantiate(items[epicSkillNum], this.transform.position, this.transform.rotation);
                }

            }


        }

        else if(mMonsterType == EMonsterType.Elite)
        {
            // 엘리트는 무조건 스킬 하나는 생성

            int randNum3 = Random.Range(0, 100);

            if (randNum3 >= 0 && randNum3 < 70) // 70퍼 : 에픽 스킬 생성
            {
                int epicSkillNum = Random.Range(4, 7); 

                Instantiate(items[epicSkillNum], this.transform.position, this.transform.rotation);
            }
            else  // 30퍼 : 유니크 스킬 생성
            {
                int epicSkillNum = Random.Range(7, 9); 

                Instantiate(items[epicSkillNum], this.transform.position, this.transform.rotation);
            }
          
        }

        else if (mMonsterType == EMonsterType.Boss)
        {
            // 보스는 확률적으로 여러개를 생성

            int scrollNum = Random.Range(1, 4); // 1개부터 3까지 생성

            for(int i = 0; i < scrollNum; i++) 
            {
                int randNum4 = Random.Range(0, 100);

                if(randNum4 >= 0 && randNum4 < 30) // 30퍼 : 에픽 스킬 생성
                {
                    int epicSkillNum = Random.Range(4, 7);

                    Instantiate(items[epicSkillNum], this.transform.position + new Vector3(0, 0, -i), this.transform.rotation); // 임시
                }
                else // 70퍼 : 유니크 스킬 생성
                {
                    int specialSkillNum = Random.Range(7, 9); // 7부터 8까지 나오면 해당 스킬 생성

                    Instantiate(items[specialSkillNum], this.transform.position + new Vector3(0, 0, -i), this.transform.rotation); // 임시
                }
            }
          

        }
    }

    public void DropBuff()
    {

    }
}
