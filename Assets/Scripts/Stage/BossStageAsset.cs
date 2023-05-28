using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage / Create New Boss Stage")]
public class BossStageAsset : StageAsset
{
    public BossStageAsset()
    {
        gameMode = GameMode.Boss;
    }
    public override bool IsClear()
    {
        if (GameManager.instance.monsters[0].CurHealth > 0)
        {
            return false;
        }
        return true;
    }
    public override bool IsOver()
    {
        if (GameManager.instance.player.CurHealth > 0)
        {
            return false;
        }
        return true;
    }
}
