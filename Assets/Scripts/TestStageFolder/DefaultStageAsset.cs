﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Stage / Create New Default Stage")]
public class DefaultStageAsset : StageAsset
{
    public DefaultStageAsset()
    {
        gameMode = GameMode.Default;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override bool IsClear()
    {
        if(GameManager.instance.currentStageMonsterCount > 0)
        {
            return false;
        }
        return true;
    }
}
