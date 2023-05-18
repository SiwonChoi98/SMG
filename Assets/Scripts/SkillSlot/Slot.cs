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
    private int itemMaxCount = 4; //획득한 아이템의 최대 개수
    public Image itemCountImage; //획득한 아이템의 개수 보여주는 이미지
    public float maxBuffTime; // 버프 타임의 최대값
    public float cloneMaxButtTime; //버프타임 이미지의 최대값
    public Image itemImage; // 아이템의 이미지
    public BaseSkill slotSkill; // Slot에 들어가는 스킬 정보, 스킬을 얻은 경우에만 추가
    public BaseBuff slotBuff; // Buff에 들어가는 버프 정보, 버프를 얻은 경우에만 추가 

    [SerializeField]
    private Player player; // Slot에게 할당된 플레이어
    [SerializeField]
    private int player_DefualtStrength;
    [SerializeField]
    private GameObject go_CountImage; // 스킬 사용 횟수 뒤에 보이는 배경 이미지
    [SerializeField]
    private Image go_BuffTimeImage; // 버프 지속 시간 뒤에 보이는 배경 이미지
    [SerializeField]
    private Text text_Count; // 스킬 사용 횟수 Text
    [SerializeField]
    private Text text_BuffTime; // 버프 지속 시간 Text

    private void Start()
    {
        orginPos = transform.position;

        maxBuffTime = 0f;

        player = SkillManager.instance.player; // 스킬 매니저로부터 플레이어를 받아온다.

        player_DefualtStrength = player.Strength; // 원래 플레이어의 공격력 수치 초기값을 받아온다.
        
    }

    private void Update()
    {
        if(item != null && item.itemType == Item.EItemType.Buff) // 현재 슬롯에 아이템이 있고, 아이템이 버프 타입이라면 시간 감소 실행
        {
            if (maxBuffTime > 0) // 지속형 버프
            {
                maxBuffTime -= Time.deltaTime;
                text_BuffTime.text = ((int)maxBuffTime).ToString();
                go_BuffTimeImage.fillAmount -= (float)1 / (int)cloneMaxButtTime * Time.deltaTime;
                // Debug.Log("버프 타임 : " + maxBuffTime);
            }
            else
            {
                ClearBuffSlot();
            }
        }
        


    }
    // 이미지의 투명도 조절
    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(Item _item, int _count = 1) // 현재는 하나씩 더해주고 있는데, 이 개수도 조정 가능해야 한다.
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage; // 스프라이트로 형변환

        if (item.itemType == Item.EItemType.Skill) // 아이템이 스킬형인 경우
        {
            slotSkill = _item.skill; // 아이템에 있던 스킬 정보를 넣어준다.
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
            if (itemCount > itemMaxCount)
                return;
            
            itemCountImage.fillAmount = (float)itemCount / (float)itemMaxCount;//Mathf.Lerp(itemCountImage.fillAmount, (float)itemCount / 4 / 1 / 1 ,Time.deltaTime * 5); 
        }
        else if(item.itemType== Item.EItemType.Buff) // 아이템이 버프형인 경우
        {
            slotBuff = _item.buff;
            maxBuffTime = slotBuff.buffTimeMax; // 각 버프의 버프 시간만큼 버프를 준다.
            slotBuff.ExcuteBuff(player.gameObject); // 해당 버프에 있는 ExcuteBuff 를 실행하면, 버프매니저로 실행. 중복해서 먹었을 때 버프가 중복해서 걸리지 않도록 하기 위함.
            go_BuffTimeImage.gameObject.SetActive(true);
            go_BuffTimeImage.fillAmount = 1;
            text_BuffTime.text = ((int)maxBuffTime).ToString(); // 시간은 float 형태가 아닌 우선 int 형태로 출력하게 함
            cloneMaxButtTime = maxBuffTime;
        }

        SetColor(1);
    
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        if (itemCount > itemMaxCount)
            return;
        itemCountImage.fillAmount = (float)itemCount / (float)itemMaxCount;
        //itemCountImage.fillAmount = (float)_count;//Mathf.Lerp(itemCountImage.fillAmount, (float)itemCount / 4 ,Time.deltaTime * 5);
        if (itemCount <= 0)
        {
            ClearSkillSlot();
            itemCountImage.fillAmount = 0f;
        }    
    }

    // 버프 시간 조정
    public void SetSlotTime(float _time)
    {
        maxBuffTime = _time; // 더해주는 것이 아닌 다시 초기화
        BuffManager.instance.SlotBuffReset(slotBuff.mBuffType);
        text_BuffTime.text = ((int)maxBuffTime).ToString();
        go_BuffTimeImage.fillAmount = 1;
    }

    // 스킬 슬롯 초기화
    private void ClearSkillSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        // slotSkill은 null처리 하면 스킬이 다 실행되기 전에 사라져서 터진다.
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);


    }

    // 버프 슬롯 초기화
    private void ClearBuffSlot()
    {
        item = null;
        maxBuffTime = 0f;
        itemImage.sprite = null;

        SetColor(0);

        text_BuffTime.text = "0";
        go_BuffTimeImage.gameObject.SetActive(false);
        go_BuffTimeImage.fillAmount = 1;
    }

    // 이 부분을 나중에 마우스 클릭이 아닌 모바일 터치로 바꿔보자.
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            
            if(item != null && item.itemType == Item.EItemType.Skill) // 슬롯에 들어있는 아이템이 스킬인 경우에만 사용
            {
                if (player.IsPlayerCanUseSkill) // 플레이어가 !isAttacking && !isDodging && !isCasting 인 상태이면
                {
                    Debug.Log(item.itemName + " 스킬을 사용했습니다.");

                    SetSlotCount(-1);
                    player.UseSlotSkill(slotSkill); // 플레이어에게 스킬 사용하게 만든다. 여기서 계속 오류가 났었는데, 위에 처럼 public 변수가 없어서 그랬던 것이다.
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
