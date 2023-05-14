using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // 인벤토리를 눌렀을 경우 다른 공격이나 이동 기능을 막는 것, 우리 게임에는 딱히..?, 왜냐하면 우리는 공격 버튼은 따로 있어서 괜찮을 것 같기도 하다.

    // 인벤토리 키를 눌렀을 경우 인벤토리 활성화, 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;

    [SerializeField] 
    private GameObject go_SkillSlot; // 스킬 슬롯들의 부모 오브젝트
    [SerializeField]
    private GameObject go_BuffSlot; // 버프 슬롯들의 부모 오브젝트

    [SerializeField]
    private Slot[] skillSlots; // 스킬 슬롯들
    [SerializeField]
    private Slot[] buffSlots; // 버프 슬롯들


    // Start is called before the first frame update
    private void Start()
    {
        skillSlots = go_SkillSlot.GetComponentsInChildren<Slot>();
        buffSlots = go_BuffSlot.GetComponentsInChildren<Slot>();
    }

    // 이 부분에서 스킬 슬롯이 다 찼는지, 또는 스킬을 먹었는데 보유한 스킬 개수가 이미 Max인지 검사해야 한다.
    
    public void AcquireItem(Item _item, GameObject _dropItem, int _count = 1)
    {
        if(_item.itemType == Item.EItemType.Skill) // 획득한 아이템이 스킬인 경우
        {
            for (int i = 0; i < skillSlots.Length; i++) // 슬롯에 이미 같은 스킬이 있는 경우 SetSlotCount로 개수를 늘려준다.
            {
                if (skillSlots[i].item != null)
                {
                    if (skillSlots[i].item.itemName == _item.itemName) // 같은 스킬을 획득한 경우
                    {
                        if(skillSlots[i].itemCount < 4) // 개수가 4보다 작은 경우에만 count를 넣어준다.
                        {
                            // Default가 1인 상태인데, 여기서 _count를 바꾸면 된다.
                            skillSlots[i].SetSlotCount(_count);

                            Destroy(_dropItem);
                            
                            return;
                        }
                        else
                        {
                            Debug.Log(skillSlots[i].item.itemName + " 스킬 제한 개수를 초과하였습니다!");
                        }
                       
                    }

                    else
                    {
                        if(i == skillSlots.Length - 1) // 끝까지 검사를 다한 경우
                        {
                            Debug.Log("비어있는 스킬 슬롯이 없습니다!");
                        }
                    }

                }
               
            }

            for (int i = 0; i < skillSlots.Length; i++) // 슬롯에 아이템이 없으면, 빈 공간에 넣어준다.
            {
                if (skillSlots[i].item == null)
                {
                    skillSlots[i].AddItem(_item, _count);

                    Destroy(_dropItem);

                    return;
                }
            }
        }

        else if (_item.itemType == Item.EItemType.Buff) // 획득한 아이템이 버프인 경우
        {
            
            for(int i = 0; i < buffSlots.Length; i++) // 슬롯에 이미 같은 버프가 있는 경우 SetSlotTime으로 해당 시간을 다시 초기화 시켜준다.
            {
                if (buffSlots[i].item != null) 
                {
                    if (buffSlots[i].item.itemName == _item.itemName)
                    {
                        // Default가 15인 상태인데, 여기서 _time을 바꾸면 된다.
                        buffSlots[i].SetSlotTime(buffSlots[i].item.buff.buffTimeMax);
                        Destroy(_dropItem);
                        return;
                    }
                }
            }

            for (int i = 0; i < buffSlots.Length; i++) // 슬롯에 아이템이 없으면, 빈 공간에 넣어준다.
            {
                if (buffSlots[i].item == null)
                {
                    buffSlots[i].AddItem(_item, _count);
                    Destroy(_dropItem);
                    return;
                }
            }
        }
            
       
    }
}
