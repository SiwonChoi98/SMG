using System.Collections;
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

    public int currentMonsterCount; //몬스터 죽을때 줄여줄 카운트
    public int spawnCount; //현재 스테이지 스폰 몬스터 수
    private int spawnIndex; 
    public float currentStageRespawnTime; //현재 스테이지 리스폰타임
    [SerializeField] private Transform monsterSpawnPos; //몬스터 나오는 위치
    [SerializeField] private Text currentStageTxt; //현재 스테이지 텍스트
    [SerializeField] private TextMeshProUGUI currentMonsterCountTxt; //현재 몬스터 수 텍스트
    [SerializeField] private Transform outPortalPos; //스테이지 시작 위치
    [SerializeField] private GameObject clearPortal; //클리어 포탈
    

    public void NextGame() 
    {
        StageManager.instance.SetCurrent(++StageManager.instance.currentStageIndex);
        currentStageTxt.text = StageManager.instance.currentStageIndex.ToString();
        player.transform.position = outPortalPos.position; //시작위치조정
        SceneManager.LoadSceneAsync("InGame");
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        spawnIndex = 0;
        currentStageTxt.text = StageManager.instance.currentStageIndex.ToString();
        player.transform.position = outPortalPos.position; //시작위치조정
        clearPortal.SetActive(false); //스테이지 시작 시 클리어 포탈 비활성화
    }
    private void Update()
    {
        if (stage.asset.IsClear())
        {
            Debug.Log("StageClear");
            clearPortal.SetActive(true);

        }
        if (stage.asset.IsOver())
        {
            Debug.Log("GameOver");
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
                Monster monster = Instantiate(stage.asset.monsters[spawnIndex]); //소환
                monster.transform.position = monsterSpawnPos.position;
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
