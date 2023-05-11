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
    private void Awake()
    {
        titleAnim = titlePlayer.GetComponentInChildren<Animator>();
        //SoundManager.instance.BgmPlaySound(0); //테스트
    }
    private void Start()
    {
        bestScoreTxt.text = StageManager.instance.lastStageIndex.ToString() + " STAGE";
    }
    public void StartButton()
    {
        StartCoroutine(StartStage());
    }
    private IEnumerator StartStage()
    {
        titleAnim.SetTrigger("isStartMotion");
        StageManager.instance.SetCurrent(1);
        yield return new WaitForSeconds(1f);
        LoadingSceneController.Instance.LoadScene("InGame");
    }

}
