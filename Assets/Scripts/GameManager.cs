using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("플레이어 체력관련 이미지")]
    public Player player;
    [SerializeField] private Transform playerHealthImageTrans;
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Image playerCanvasHealthImage;


    public Stage stage;
    public int currentStageMonsterCount;
    public float currentStageRespawnTime;
    public Transform monsterSpawnPos;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (stage == null)
            return;
        MonsterSpwan();
    }
    public void MonsterSpwan()
    {
        if (stage.asset.IsClear())
            return;
        //클리어가 안되면 몬스터 소환
        if(currentStageRespawnTime > 0)
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
