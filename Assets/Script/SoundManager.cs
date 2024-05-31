using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public Slider VolumeSlider;
    public AudioClip[] SoundClip;
    public AudioClip[] DanceMusicClip;
    public AudioSource DanceAudioSource;
    public AudioSource SoundEffectAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DanceAudioSource = gameObject.AddComponent<AudioSource>();
            SoundEffectAudioSource = gameObject.AddComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        VolumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void OnVolumeChanged(float value)
    {
        DanceAudioSource.volume = value;
        SoundEffectAudioSource.volume = value;
    }

    public void PlaySummonSound(int index)
    {
        if (index == 0) {
            SoundEffectAudioSource.clip = SoundClip[0];
            SoundEffectAudioSource.Play();
        }
        else {
            SoundEffectAudioSource.clip = SoundClip[1];
            SoundEffectAudioSource.Play();
        }
        
    }

    public void PlayPokeballSound()
    {
        SoundEffectAudioSource.clip = SoundClip[2];
        SoundEffectAudioSource.Play();
    }

    public void PlayDanceSound(int index)
    {
        if (index == 0) {
            DanceAudioSource.clip = DanceMusicClip[0];
            DanceAudioSource.Play();
            SoundEffectAudioSource.clip = SoundClip[3];
            SoundEffectAudioSource.Play();
        }
        else {
            DanceAudioSource.clip = DanceMusicClip[1];
            DanceAudioSource.Play();
            SoundEffectAudioSource.clip = SoundClip[4];
            SoundEffectAudioSource.Play();
        }
        
    }
}
