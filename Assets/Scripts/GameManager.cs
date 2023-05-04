using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("플레이어 체력관련 이미지")]
    public Player player;
    [SerializeField] private Transform playerHealthImageTrans;
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Image playerCanvasHealthImage;


    public Stage stage;
    public int currentStageMonsterCount; //현재 스테이지 몬스터 수
    public float currentStageRespawnTime; //현재 스테이지 리스폰타임
    public Transform monsterSpawnPos; //몬스터 나오는 위치
    public Text currentStageTxt; //현재 스테이지 텍스트
    public Transform outPortalPos; //스테이지 시작 위치
    

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
        currentStageTxt.text = StageManager.instance.currentStageIndex.ToString();
        player.transform.position = outPortalPos.position; //시작위치조정
    }

    // Update is called once per frame
    private void Update()
    {
        if (stage.asset.IsClear())
        {
            Time.timeScale = 0;
        }
        if (stage.asset.IsOver())
        {
            Time.timeScale = 0;
        }
        MonsterSpwan();
    }
    public void MonsterSpwan()
    {
        if (currentStageRespawnTime > 0)
        {
            currentStageRespawnTime -= Time.deltaTime;
        }
        else
        {
            currentStageRespawnTime = stage.asset.respawnTime;
            Monster monster = Instantiate(stage.asset.monsters[0]);
            monster.transform.position = monsterSpawnPos.position;
        }


    }
    private void LateUpdate()
    {
        GUI();
    }
    private void GUI()
    {

        playerHealthImageTrans.position = player.transform.position + new Vector3(0, 2f, 0); //플레이어 체력 위치 플레이어 머리위에
        playerHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.CurHealth / player.MaxHealth / 1 / 1, Time.deltaTime * 5); //플레이어 체력
        playerCanvasHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.CurHealth / player.MaxHealth / 1 / 1, Time.deltaTime * 5); //플레이어 캔버스 체력
    }
    
}
