using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class Title : MonoBehaviour
{
    public Animator titleAnim;
    public GameObject titlePlayer;
    private void Awake()
    {
        titleAnim = titlePlayer.GetComponentInChildren<Animator>();
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
