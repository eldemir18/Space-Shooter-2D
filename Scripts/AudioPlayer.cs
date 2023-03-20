using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] AudioClip[] shootingClips;
    
    [Header("Explosion")]
    [SerializeField] AudioClip[] explosionClips;

    [Header("Background Musics")]
    [SerializeField] AudioSource audioSource;
    
    [Header("Game Musics")]
    [SerializeField] AudioClip[] gameMusics;

    [Header("Main Menu Music")]
    [SerializeField] AudioClip mainMenuMusic;

    [Header("End Game Music")]
    [SerializeField] AudioClip endGameMusic;
        
    void Awake()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Music Volume", 0.5f);
    }

    void Start()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;

        if(activeScene == 0)
        {
            audioSource.clip = mainMenuMusic;
        }
        else if(activeScene == 1)
        {
            audioSource.clip = gameMusics[0];
        }
        else
        {
            audioSource.clip = endGameMusic;
        }

        audioSource.Play();
    }

    public void ChangeMusicVolume(Slider slider)
    {
        audioSource.volume = slider.value;
        PlayerPrefs.SetFloat("Music Volume", slider.value);
    }

    public void ChangeEffectVolume(Slider slider)
    {
        PlayerPrefs.SetFloat("Effect Volume", slider.value);
    }

    public void ChangeMusic(int index)
    {
        if(index >= gameMusics.Length) return;

        audioSource.clip = gameMusics[index];
        audioSource.Play();
    }

    public void PlayShootingClip()
    {
        PlayClip(shootingClips);
    }

    public void PlayExplosionClip()
    {
        PlayClip(explosionClips);
    }

    void PlayClip(AudioClip[] clips)
    {
        int randomClip = Random.Range(0, clips.Length);
        float effectVolume = PlayerPrefs.GetFloat("Effect Volume");
        AudioSource.PlayClipAtPoint(clips[randomClip], Camera.main.transform.position, effectVolume);
    }
}
