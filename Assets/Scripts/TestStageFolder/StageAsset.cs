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
    public int monsterCount; //스테이지별 몬스터 수
    public List<Monster> monsters; //스테이지별 몬스터 프리팹
    public float respawnTime; //스테이지별 몬스터 리스폰 시간

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
        GameManager.instance.currentStageMonsterCount = monsterCount;
        GameManager.instance.currentStageRespawnTime = respawnTime;
    }

    public abstract bool IsClear(); //클리어 조건
    public virtual bool IsOver() //오버 조건
    {
        return false;
    }

}
