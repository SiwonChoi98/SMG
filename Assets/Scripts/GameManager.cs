using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Player Health")]
    public Player player;
    [SerializeField] private Transform playerHealthImageTrans;
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Image playerCanvasHealthImage;
    [SerializeField] private Image playerDodgeCoolImage;
    public Image playerShieldImage;

    [Header("Stage")]
    public Stage stage;
    public List<Monster> monsters;
    [SerializeField] private List<Transform> monsterSpawnPos; //몬스터 나오는 위치
    [SerializeField] private GameObject monsterSpawn; //몬스터 위치 부모
    public int currentMonsterCount; //몬스터 죽을때 줄여줄 카운트
    public int spawnCount; //현재 스테이지 스폰 몬스터 수
    private int spawnIndex; 
    public float currentStageRespawnTime; //현재 스테이지 리스폰타임
    
    [SerializeField] private TextMeshProUGUI currentStageTxt; //현재 스테이지 텍스트
    [SerializeField] private TextMeshProUGUI currentMonsterCountTxt; //현재 몬스터 수 텍스트
    [SerializeField] private Transform outPortalPos; //스테이지 시작 위치
    [SerializeField] private GameObject clearPortal; //클리어 포탈
    [SerializeField] private GameObject clearTxt; //클리어 텍스트
    [SerializeField] private PlayableDirector clearTimeLine;
    private int clearTxtCount; //클리어 텍스트 출현 빈도 수
    [SerializeField] private GameObject gameOverPanel; //게임오버 판넬
     
    public Slider[] volumeSlider; //볼륨조절
    public void BackSceneButton()
    {
        ClickSound();
        LoadingSceneController.Instance.LoadScene("Title");
    }
    public void NextGame()
    { 
        StageManager.instance.SetCurrent(++StageManager.instance.currentStageIndex);
        LoadingSceneController.Instance.LoadScene("InGame");
    }
    public void ReplayGame()
    {
        ClickSound();
        StageManager.instance.SetCurrent(StageManager.instance.currentStageIndex);
        LoadingSceneController.Instance.LoadScene("InGame");
    }
    public void PauseGame()
    {
        ClickSound();
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
    }
    public void ClickSound()
    {
        SoundManager.instance.SfxPlaySound(15);
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
        int ran = Random.Range(4, 8);
        SoundManager.instance.BgmPlaySound(ran); //4~7번까지 인게임 사운드 후보

        Init(); //각종 정보들
        PlayerStatInit(); //플레이어 정보

        MonsterSpawnDataSave(); //몬스터 풀 저장
        MonsterPosDataSave(); //몬스터 위치 저장
        
    }
    private void Init()
    {
        PlayGame();
        spawnIndex = 0;
        currentStageTxt.text = StageManager.instance.currentStageIndex.ToString(); //현 스테이지 보여주기
        player.transform.position = outPortalPos.position; //시작위치조정
        volumeSlider[0].value = SoundManager.instance.bgmAudioSource.volume; //볼륨조절
        volumeSlider[1].value = SoundManager.instance.sfxAudioSource.volume;
        clearPortal.SetActive(false); //스테이지 시작 시 클리어 포탈 비활성화
        clearTxtCount = 1;
    }
    private void PlayerStatInit()
    {
        if (StageManager.instance.currentStageIndex != 1)
        {
            player.CurHealth = PlayerDataManager.instance.playerCurHealth;
            player.MaxHealth = PlayerDataManager.instance.playerMaxHealth;
            player.Strength = PlayerDataManager.instance.playerStrength;
            PlayerDataManager.instance.PlayerSkillsLoad();
        }
    }
    private void MonsterSpawnDataSave()
    {
        for (int i = 0; i < stage.asset.monsters.Count; i++) //스테이지 별 풀 저장
        {
            monsters.Add(stage.asset.monsters[i]); //몬스터 
            monsters[i] = Instantiate(monsters[i]);
            monsters[i].gameObject.SetActive(false);
        }
    }
    private void MonsterPosDataSave()
    {
        for(int i=1; i< monsterSpawn.GetComponentsInChildren<Transform>().Length; i++)
        {
            monsterSpawnPos.Add(monsterSpawn.GetComponentsInChildren<Transform>()[i]);
        }
    }
    private void Update()
    {
        if (stage.asset.IsClear())
        {
            clearPortal.SetActive(true);
            if (clearTxtCount > 0)
            {
                StartCoroutine(ClearTxt());
            }
        }
        if (stage.asset.IsOver())
        {
            gameOverPanel.SetActive(true);
        }
        MonsterSpwan();
    }
    private IEnumerator ClearTxt()
    {
        clearTxt.SetActive(true);
        clearTimeLine.Play();
        yield return new WaitForSeconds(2f);
        clearTxt.SetActive(false);
        clearTimeLine.Stop();
        clearTxtCount--;
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
                int spawnRan = Random.Range(0, monsterSpawnPos.Count);
                monsters[spawnIndex].gameObject.transform.position = monsterSpawnPos[spawnRan].position;

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
        if (player.isDodgeReady)
            playerDodgeCoolImage.fillAmount = 0;
        else
            playerDodgeCoolImage.fillAmount = player.dodgeCoolTime / player.dodgeCoolTimeMax;
    }
    
}
