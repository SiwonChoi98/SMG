﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        StartCoroutine(Start11());
    }
    private IEnumerator Start11()
    {
        titleAnim.SetTrigger("isStartMotion");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/Demo_Scene/demo");
    }
}
