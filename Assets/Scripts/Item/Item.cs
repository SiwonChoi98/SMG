using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject // 게임 오브젝트에 굳이 붙이필요 없는 오브젝트
{
    public string itemName; // 아이템의 이름
    public EItemType itemType; // 스킬인지, 버프류인지, 포션인지 분류, 스킬이면 1,2,3번 슬롯에, 버프면 그냥 흡수, 포션이면 4번 슬롯에 고정으로 들어가야한다.
    public Sprite itemImage; // 아이템의 이미지, 스프라이트 형을 사용하는 이유는
    // 캔버스 필요없이 월드 상에서 이미지가 출력 가능하기 때문이다. 이미지 형은 캔버스가 있어야만 가능
    public GameObject itemPrefab; // 아이템의 프리팹, 월드 상에 실제로 떨어지는 아이템

    public BaseSkill skill; // 아이템이 가지고 있는 스킬 정보
    public BaseBuff buff; // 아이템이 가지고 있는 버프 타입
    // 이 부분에 버프 정보를 담고 있는 클래스가 필요하다. public BaseBuff skill; // 버프가 가지고 있는 버프 정보, 버프 매니저를 둬서 버프 매니저보고 틀어달라고 할까? 아니면 

    public enum EItemType
    {
        Skill,
        Buff
    }

}
