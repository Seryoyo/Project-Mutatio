using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSrc;
    public AudioClip bgMusic;
    //[SerializeField] private Slider musicSlider; // if we add slider???


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSrc = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        if (bgMusic != null) {
            PlayBgMusic(false, bgMusic);
        }
        //musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); }); // if we add slider we can use this
    }

    public static void SetVolume(float volume) {
        Instance.audioSrc.volume = volume;
    }

    public static void PlayBgMusic(bool resetSong, AudioClip audioClip = null) {
        if (audioClip != null) {
            Instance.audioSrc.clip = audioClip;
        } else if (Instance.audioSrc.clip != null) {
            if (resetSong) {
                Instance.audioSrc.Stop();
            }
            Instance.audioSrc.Play();
        }
    }

    public static void PauseBgMusic() {
        Instance.audioSrc.Pause();
    }
}
