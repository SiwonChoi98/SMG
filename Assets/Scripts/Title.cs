using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
public class Title : MonoBehaviour
{
    public Animator titleAnim;
    public GameObject titlePlayer;
    public Text bestScoreTxt;
    public bool isStart = false;
    private void Awake()
    {
        Time.timeScale = 1;
        isStart = false;
        titleAnim = titlePlayer.GetComponentInChildren<Animator>();
        //SoundManager.instance.BgmPlaySound(0); //테스트
    }
    private void Start()
    {
        bestScoreTxt.text = StageManager.instance.lastStageIndex.ToString() + " STAGE";
    }
    public void StartButton()
    {
        if(!isStart)
            StartCoroutine(StartStage());
        isStart = true;
    }
    private IEnumerator StartStage()
    {
        titleAnim.SetTrigger("isStartMotion");
        StageManager.instance.SetCurrent(1);
        yield return new WaitForSeconds(1f);
        LoadingSceneController.Instance.LoadScene("InGame");
    }

}
