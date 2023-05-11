using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    private const string CURRENT_STAGE_KEY = "CURRENT_STAGE"; 

    public List<StageAsset> stageAssets;
    public StageAsset currentStage;
    public int currentStageIndex; //현재스테이지
    public int lastStageIndex; //제일 마지막 스테이지
    [SerializeField]
    private string _folderName = "Stage";
    private void Awake()
    {
       
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);        

        stageAssets = new List<StageAsset>();
       

    }
    private void Start()
    {
        //PlayerPrefs.SetInt(CURRENT_STAGE_KEY, lastStageIndex = 0); //최고기록 초기화
        lastStageIndex = PlayerPrefs.GetInt(CURRENT_STAGE_KEY);
        foreach (var stage in Resources.LoadAll(_folderName))
        {
            stageAssets.Add((StageAsset)stage);
        }
    }
    // Update is called once per frame
    public void LastStageUp()
    {
        if (currentStageIndex >= lastStageIndex)
        {
            lastStageIndex = currentStageIndex;
            PlayerPrefs.SetInt(CURRENT_STAGE_KEY, lastStageIndex);
        }


    }
    public StageAsset GetCurrentStage()
    {
        return currentStage;
    }
    public void SetCurrent(int num)
    {
        currentStageIndex = num;
        var c = from stage in stageAssets
                where stage.name.Equals("Stage" + currentStageIndex)
                select stage;

        foreach (var stage in c)
        {
            currentStage = stage;
            break;
        }
    }
}
