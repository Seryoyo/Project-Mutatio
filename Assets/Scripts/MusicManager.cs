using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource layerSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Clips")]
    public AudioClip bg;
    public AudioClip heartbeat;
    public AudioClip userShoot;
    public AudioClip bossMusic;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.clip = bg;
        musicSource.loop = true;
        musicSource.volume = 0.1f;
        musicSource.Play();
        layerSource.clip = heartbeat;
        layerSource.loop = true;
        layerSource.volume = 0.2f;
        layerSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void BossChange(AudioClip clip)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
        layerSource.volume = 0.4f;
    }

    public void resetAudio(AudioClip clip) {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
        layerSource.volume = 0.2f;
    }
}