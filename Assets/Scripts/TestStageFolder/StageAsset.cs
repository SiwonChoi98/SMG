using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum GameMode
{
    Default,
    Boss
}
public abstract class StageAsset : ScriptableObject
{
    public GameMode gameMode;
    public int monsterCount; //몬스터 수
    public List<Monster> monsters; //몬스터 종류
    public float respawnTime; //몬스터 리스폰 시간

    public StageAsset()
    {
        gameMode = GameMode.Default;
    }
    private void OnEnable()
    {
        EditorUtility.SetDirty(this);
    }
    public virtual void Initialize()
    {
        GameManager.instance.currentMonsterCount = monsterCount;
        GameManager.instance.spawnCount = monsterCount;

        GameManager.instance.currentStageRespawnTime = respawnTime;
        
    }

    public abstract bool IsClear(); //클리어 조건
    public virtual bool IsOver() //오버 조건
    {
        return false;
    }

}
