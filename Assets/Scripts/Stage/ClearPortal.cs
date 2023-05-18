using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.instance.LastStageUp();
            PlayerDataManager.instance.PlayerStatSave(GameManager.instance.player.CurHealth, GameManager.instance.player.MaxHealth, GameManager.instance.player.Strength);
            PlayerDataManager.instance.PlayerSkillsSave();
            GameManager.instance.NextGame();
        }
    }
}
