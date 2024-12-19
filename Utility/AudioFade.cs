using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFade : MonoBehaviour
{
    private AudioFade instance;
    public SoundMixerManager soundManager;
    public AudioMixer mixer;
    [SerializeField] private AudioSource sceneAudio;
    public AudioClip sceneMusic;
    private bool isPlaying = false;
    public bool playOnStart = false;

    public float fadeTime = 1.5f;
    public float timeElapsed = 0f;
    public float bgmVolume;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundMixerManager>();
        LoadVolume();
    }

    private void Start()
    {
        if(playOnStart == true)
        {
            sceneAudio.Play();
            FadeMusic(mixer);
        }
        else
        {

        }
    }

    private void LoadVolume()
    {
        bgmVolume = soundManager.musicVolume;
        mixer.SetFloat("bgmVolume", Mathf.Log10(bgmVolume) * 20);
    }

    public void SetBool()
    {
        isPlaying = !isPlaying;
    }

    public void FadeMusic(AudioMixer fading)
    {
        LoadVolume();
        StopAllCoroutines();
        timeElapsed = 0;
        StartCoroutine(FadingAudio(fading));
        isPlaying = !isPlaying;
    }

    private IEnumerator FadingAudio(AudioMixer fading)
    {
        if (!isPlaying)
        {
            
            while (timeElapsed < fadeTime)
            {
                sceneAudio.volume = Mathf.Lerp(0, 1, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (timeElapsed < fadeTime)
            {
                sceneAudio.volume = Mathf.Lerp(1, 0, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

}
