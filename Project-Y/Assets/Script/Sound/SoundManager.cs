using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null || bgmSource == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }

    public void SFXPlay(string name, AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        if (bgmSource != null)
            bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}