using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundSource;
    [SerializeField] float maxMusicVolume;
    [SerializeField] Slider musicSlider;
    [SerializeField] float maxSoundVolume;
    [SerializeField] Slider soundSlider;

    //Music
    [SerializeField] AudioClip backgroundMusic;

    //Sounds
    [SerializeField]AudioClip errorSound;
    [SerializeField]AudioClip confirmSound;

    [SerializeField]AudioClip buySound;
    [SerializeField] AudioClip sellSound;

    [SerializeField]AudioClip sowSound;
    [SerializeField]AudioClip waterSound;

    [SerializeField]AudioClip dragPotSound;
    [SerializeField]AudioClip dropPotSound;

    [SerializeField] AudioClip sleepSound;



    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();

        musicSlider.maxValue = maxMusicVolume;
        soundSlider.maxValue = maxSoundVolume;

        if(PlayerPrefs.GetFloat("MusicVolume") == 0f)
        {
            PlayerPrefs.SetFloat("MusicVolume", maxMusicVolume / 2);
        }

        if (PlayerPrefs.GetFloat("SoundVolume") == 0f)
        {
            PlayerPrefs.SetFloat("SoundVolume", maxSoundVolume / 2);
        }

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");

        GameEvents.Instance.Error.AddListener(() => PlaySound(errorSound));
        GameEvents.Instance.Confirm.AddListener(() => PlaySound(confirmSound));
        GameEvents.Instance.Pause.AddListener(() => RefreshSlider());

        GameEvents.Instance.BuyItem.AddListener(() => PlaySound(buySound));
        GameEvents.Instance.SellPlant.AddListener(() => PlaySound(sellSound));

        GameEvents.Instance.SowPlant.AddListener(() => PlaySound(sowSound));
        GameEvents.Instance.WaterPlant.AddListener(() => PlaySound(waterSound));

        GameEvents.Instance.DragPot.AddListener(() => PlaySound(dragPotSound));
        GameEvents.Instance.DropPot.AddListener(() => PlaySound(dropPotSound));

        GameEvents.Instance.NewDay.AddListener(() => PlaySound(sleepSound));
    }

    private void RefreshSlider()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
    }

    private void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void ChangeSoundVolume()
    {
        soundSource.volume = soundSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", soundSource.volume);
    }

    public void ChangeMusicVolume()
    {
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
    }

}
