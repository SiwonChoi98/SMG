using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackUpSlot : MonoBehaviour
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Sprite itemImage; // 아이템의 이미지
    public BaseSkill slotSkill; // Slot에 들어가는 스킬 정보, 스킬을 얻은 경우에만 추가
}
