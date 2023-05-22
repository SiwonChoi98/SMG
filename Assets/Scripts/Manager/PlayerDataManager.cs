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

   
    public BackUpSlot[] slots;
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
    public void PlayerStatSave(int hp, int maxHp, int strength)
    {
        playerCurHealth = hp;
        playerMaxHealth = maxHp;
        playerStrength = strength;
    }
    public void PlayerStatLoad()
    {
        GameManager.instance.player.CurHealth = playerCurHealth;
        GameManager.instance.player.MaxHealth = playerMaxHealth;
        GameManager.instance.player.Strength = playerStrength;
    }
    public void PlayerSkillsSave()
    {
        for(int i=0; i<4; i++)
        {
            slots[i].item = Inventory.instance.skillSlots[i].item;
            slots[i].itemCount = Inventory.instance.skillSlots[i].itemCount;
            slots[i].itemImage = Inventory.instance.skillSlots[i].itemImage.sprite;
            slots[i].slotSkill = Inventory.instance.skillSlots[i].slotSkill;
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
}
