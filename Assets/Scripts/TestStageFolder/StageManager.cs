using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public List<StageAsset> stageAssets;
    public StageAsset currentStage;
    public int currentStageIndex;

    [SerializeField]
    private string _folderName = "Stage";
    private void Awake()
    {
        if(instance == null)
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
        foreach (var stage in Resources.LoadAll(_folderName))
        {
            stageAssets.Add((StageAsset)stage);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
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
