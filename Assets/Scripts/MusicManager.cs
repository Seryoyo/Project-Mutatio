using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {
    MusicManager musicManager;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource layerSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Clips")]
    public AudioClip bg;
    public AudioClip heartbeat;
    public AudioClip userShoot;

    public void Awake()
    {
        if (musicManager == null)
        {
            musicManager = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        musicSource.clip = bg;
        musicSource.loop = true;
        musicSource.volume = 0.1f;
        musicSource.Play();
        layerSource.clip = heartbeat;
        layerSource.loop = true;
        layerSource.volume = 0.2f;
        layerSource.Play();
    }

    public void PlaySFX(AudioClip clip) {
        SFXSource.PlayOneShot(clip);
    }
}