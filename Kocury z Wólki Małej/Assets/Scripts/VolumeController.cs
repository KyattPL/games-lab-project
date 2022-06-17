using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject audioController;
    private AudioSource bgAudioSource;
    void Start()
    {
        bgAudioSource = audioController.GetComponent<AudioSource>();
        bgAudioSource.volume = PlayerPrefs.GetFloat("Volume", 0.202f);
    }
    public void VolumeSlider()
    {
        bgAudioSource.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", bgAudioSource.volume);
    }
}
