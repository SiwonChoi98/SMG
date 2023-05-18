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

    public Slot[] skillSlots; // 스킬 슬롯들
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
    
}
