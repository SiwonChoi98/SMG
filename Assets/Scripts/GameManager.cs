﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Player Health")]
    public Player player;
    [SerializeField] private Transform playerHealthImageTrans;
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Image playerCanvasHealthImage;

    [Header("Stage")]
    public Stage stage;
    public List<Monster> monsters;

    public int currentMonsterCount; //몬스터 죽을때 줄여줄 카운트
    public int spawnCount; //현재 스테이지 스폰 몬스터 수
    private int spawnIndex; 
    public float currentStageRespawnTime; //현재 스테이지 리스폰타임
    [SerializeField] private Transform monsterSpawnPos; //몬스터 나오는 위치
    [SerializeField] private TextMeshProUGUI currentStageTxt; //현재 스테이지 텍스트
    [SerializeField] private TextMeshProUGUI currentMonsterCountTxt; //현재 몬스터 수 텍스트
    [SerializeField] private Transform outPortalPos; //스테이지 시작 위치
    [SerializeField] private GameObject clearPortal; //클리어 포탈
    [SerializeField] private GameObject gameOverPanel;
    public void BackSceneButton()
    {
        LoadingSceneController.Instance.LoadScene("Title");
    }
    public void NextGame() 
    {
        StageManager.instance.SetCurrent(++StageManager.instance.currentStageIndex);
        LoadingSceneController.Instance.LoadScene("InGame");
    }
    public void ReplayGame()
    {
        StageManager.instance.SetCurrent(StageManager.instance.currentStageIndex);
        LoadingSceneController.Instance.LoadScene("InGame");
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        monsters = new List<Monster>();
}
    private void Start()
    {
        PlayGame();
        spawnIndex = 0;
        currentStageTxt.text = StageManager.instance.currentStageIndex.ToString();
        player.transform.position = outPortalPos.position; //시작위치조정
        clearPortal.SetActive(false); //스테이지 시작 시 클리어 포탈 비활성화
        for(int i=0; i<stage.asset.monsters.Count; i++) //스테이지 별 풀 저장
        {
            monsters.Add(stage.asset.monsters[i]); //몬스터 
            monsters[i] = Instantiate(monsters[i]);
            monsters[i].gameObject.SetActive(false);
        }
        
    }
    private void Update()
    {
        if (stage.asset.IsClear())
        {
            clearPortal.SetActive(true);
        }
        if (stage.asset.IsOver())
        {
            gameOverPanel.SetActive(true);
            PauseGame();
        }
        MonsterSpwan();
    }
    public void MonsterSpwan()
    {
        if(spawnCount > 0) //몬스터 생성 수가 0보다 클때만 생성 
        {
            if (currentStageRespawnTime > 0)
            {
                currentStageRespawnTime -= Time.deltaTime;
            }
            else
            {
                currentStageRespawnTime = stage.asset.respawnTime;

                monsters[spawnIndex].gameObject.SetActive(true);
                monsters[spawnIndex].gameObject.transform.position = monsterSpawnPos.position;

                spawnCount--;
                spawnIndex++;
            }
        }    
    }
    private void LateUpdate()
    {
        GUI();
    }
    private void GUI()
    {
        currentMonsterCountTxt.text = currentMonsterCount.ToString(); //현재 스테이지 잡아야 할 몬스터 수
        playerHealthImageTrans.position = player.transform.position + new Vector3(0, 2f, 0); //플레이어 체력 위치 플레이어 머리위에
        playerHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.CurHealth / player.MaxHealth / 1 / 1, Time.deltaTime * 5); //플레이어 체력
        playerCanvasHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.CurHealth / player.MaxHealth / 1 / 1, Time.deltaTime * 5); //플레이어 캔버스 체력
    }
    
}
