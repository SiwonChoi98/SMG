using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageAsset asset;
    void Start()
    {
        asset = StageManager.instance.GetCurrentStage();
        if (asset)
            asset.Initialize();
        else
            Destroy(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
