using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler //IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler // using UnityEngine.EventSystems 의 인터페이스 사용

{
    private Vector3 orginPos; // 아이템의 원래 위치
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    [SerializeField]
    private GameObject go_CountImage;
    [SerializeField]
    private Text text_Count;


    private void Start()
    {
        orginPos = transform.position;
    }

    // 이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage; // 스프라이트로 형변환

        if(item.itemType != Item.EItemType.Buff) // 아이템 타입이 버프인 경우에는 개수 표시를 할 필요가 없고, 쿨타임 표시를 해줘야 한다. 이 부분은 뒤에 따로 만들자.
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = " 0 "; // 이 부분을 수정해야할듯
            go_CountImage.SetActive(false);
        }
       
        SetColor(1);
    
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }    
    }

    // 슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
        
    }

    // 이 부분을 나중에 마우스 클릭이 아닌 모바일 터치로 바꿔보자.
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null) 
            { 
                if(item.itemType == Item.EItemType.Skill) 
                {
                    // 여기에 스킬을 누르더라도 사용이 안되게 플레이어의 상태를 받아와야 한다. 예를 들어서 IsPlayerCanUseSkill을 하면 실행해주면서 카운트를 깎자.
                    // IsPlayerCanUseSkill -> !isAttacking && !isDodging && !isCasting 와 같이 만들자.
                    Debug.Log(item.itemName + " 스킬을 사용했습니다.");
                    
                    SetSlotCount(-1);
                }
                else if(item.itemType == Item.EItemType.Potion)
                {
                    Debug.Log(item.itemName + " 포션을 사용했습니다.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    //// 드래그를 시작할 때
    //public void OnBeginDrag(PointerEventData eventData) 
    //{
    //    if(item != null) 
    //    {
    //        DragSlot.instance.dragSlot = this;
    //        DragSlot.instance.DragSetImage(itemImage);

    //        DragSlot.instance.transform.position = eventData.position;
    //    }
        
    //}

    //// 드래그 할 때
    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (item != null)
    //    {
    //        DragSlot.instance.transform.position = eventData.position;
    //    }
    //}

    //// 드래그가 끝날 때
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    DragSlot.instance.SetColor(0);
    //    DragSlot.instance.dragSlot = null;
    //}

    //public void OnDrop(PointerEventData eventData)
    //{
       
    //}
}
