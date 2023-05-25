using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player _player = GameManager.instance.player;       
            PlayerDataManager.instance.PlayerStatSave(_player.CurHealth, _player.MaxHealth, _player.stat_Attack, 
                             _player.isShield, _player.ShieldCount);
            PlayerDataManager.instance.PlayerItemSave();

            StageManager.instance.LastStageUp();
            GameManager.instance.NextGame();
        }
    }
}
