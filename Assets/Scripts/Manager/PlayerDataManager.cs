using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    [Header("SaveStat")]
    public int playerCurHealth;
    public int playerMaxHealth;
    public int playerStrength;
    public bool playerIsShield; // 쉴드 상태인가?
    public int playerShieldCount; // 쉴드 스킬 시 생성하는 보호막 개수
    //public bool playerIsStrengthBuff; // 힘 버프를 받고 있는가?
    //public bool playerIsSpeedBuff; // 스피드 버프를 받고 있는가?

    public BackUpSlot[] slots; //스킬 빽업
    public BackUpSlot[] buffSlots; //버프 빽업
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

    }
    public void PlayerStatSave(int hp, int maxHp, int stat_Attack, bool isShield, int shieldCount)
    {
        playerCurHealth = hp;
        playerMaxHealth = maxHp;
        playerStrength = stat_Attack;
        playerIsShield = isShield;
        playerShieldCount = shieldCount;
        
    }
    public void PlayerStatLoad()
    {
        Player _player = GameManager.instance.player;
        _player.CurHealth = playerCurHealth;
        _player.MaxHealth = playerMaxHealth;
        _player.Strength = playerStrength;
        _player.isShield = playerIsShield;
        _player.ShieldCount = playerShieldCount;
       
    }
    public void PlayerItemSave()
    {
        for(int i=0; i<4; i++)
        {   //스킬
            slots[i].item = Inventory.instance.skillSlots[i].item;
            slots[i].itemCount = Inventory.instance.skillSlots[i].itemCount;
            slots[i].itemImage = Inventory.instance.skillSlots[i].itemImage.sprite;
            slots[i].slotSkill = Inventory.instance.skillSlots[i].slotSkill;
            //버프
            buffSlots[i].item = Inventory.instance.buffSlots[i].item;
            buffSlots[i].itemImage = Inventory.instance.buffSlots[i].itemImage.sprite;
            buffSlots[i].slotBuff = Inventory.instance.buffSlots[i].slotBuff;
            buffSlots[i].buffTime = Inventory.instance.buffSlots[i].currentBuffTime;
        }
    }
    public void PlayerSkillsLoad()
    {
        for (int i = 0; i < 4; i++)
        {
            Inventory.instance.skillSlots[i].item = slots[i].item;
            Inventory.instance.skillSlots[i].itemCount = slots[i].itemCount;
            Inventory.instance.skillSlots[i].itemImage.sprite = slots[i].itemImage;
            Inventory.instance.skillSlots[i].slotSkill = slots[i].slotSkill;
            if(slots[i].item == true)
            {
                Inventory.instance.skillSlots[i].SetColor(1);
                Inventory.instance.skillSlots[i].itemCountImage.fillAmount = (float)slots[i].itemCount / (float)4;
            }
        }
    }
    public void PlayerBuffsLoad()
    {
        for (int i = 0; i < 4; i++)
        {
            Inventory.instance.buffSlots[i].item = buffSlots[i].item;
            Inventory.instance.buffSlots[i].itemImage.sprite = buffSlots[i].itemImage;
            Inventory.instance.buffSlots[i].slotBuff = buffSlots[i].slotBuff;
            Inventory.instance.buffSlots[i].currentBuffTime = buffSlots[i].buffTime;
            

            if (Inventory.instance.buffSlots[i].item == true)
            {
                if(Inventory.instance.buffSlots[i].item.buff.mBuffType == EBuffType.StrengthBuff)
                {
                    Inventory.instance.buffSlots[i].maxBuffTime = BuffManager.instance.strengthBuffTimeMax;
                    BuffManager.instance.Buff(EBuffType.StrengthBuff, GameManager.instance.player.gameObject, 15f);
                    BuffManager.instance.strengthBuffTime = Inventory.instance.buffSlots[i].currentBuffTime;
                }

                else if (Inventory.instance.buffSlots[i].item.buff.mBuffType == EBuffType.SpeedBuff)
                {
                    Inventory.instance.buffSlots[i].maxBuffTime = BuffManager.instance.speedBuffTimeMax;
                    BuffManager.instance.Buff(EBuffType.SpeedBuff, GameManager.instance.player.gameObject, 15f);
                    BuffManager.instance.speedBuffTime = Inventory.instance.buffSlots[i].currentBuffTime;
                }
                Inventory.instance.buffSlots[i].go_BuffTimeImage.gameObject.SetActive(true);
                Inventory.instance.buffSlots[i].SetColor(1);
            }

        }
    }
}
