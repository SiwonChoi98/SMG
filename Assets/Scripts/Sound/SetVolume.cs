﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetVolume : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public void BgmSlider()
    {
        SoundManager.instance.GetComponentsInChildren<AudioSource>()[0].volume = slider.value;
    }
    public void SfxSlider()
    {
        SoundManager.instance.GetComponentsInChildren<AudioSource>()[1].volume = slider.value;
    }
}
