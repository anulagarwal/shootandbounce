using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip soundClip;
        public SoundType type;
    }
    public enum SoundType
    {
        Pop,
        Pistol,
        ShotGun,
        Lvl3,
        Lvl4,
        Lvl5,
        Lvl6,
        Lvl7

        // Add more sound types here
    }

    public static SoundManager Instance = null;

    [SerializeField] private List<Sound> sounds;
    [SerializeField] private int poolSize = 5;
    private Queue<AudioSource> audioSourcePool;
    public float soundVolume =1f;

    private void Awake()
    {
        Application.targetFrameRate = 100;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        InitializeAudioSourcePool();
    }
    public void UpdateSound(float f)
    {
        soundVolume = f;
    }

    private void InitializeAudioSourcePool()
    {
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            audioSourcePool.Enqueue(newAudioSource);
        }
    }

    public void Play(SoundType type)
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            AudioClip clipToPlay = sounds.Find(x => x.type == type).soundClip;
            AudioSource availableSource = GetAvailableAudioSource();
            availableSource.clip = clipToPlay;
            availableSource.Play();
        }
    }
    public void Play(int level)
    {
        SoundType type = SoundType.Pistol;
        if(level == 1)
        {
            type = SoundType.Pistol;
        }
        if (level == 2)
        {
            type = SoundType.ShotGun;
        }
        if (level == 3)
        {
            type = SoundType.Lvl3;
        }
        if (level == 4)
        {
            type = SoundType.Lvl4;
        }

        if (level == 5)
        {
            type = SoundType.Lvl5;
        }

        if (level == 6)
        {
            type = SoundType.Lvl6;
        }
        if (level == 7)
        {
            type = SoundType.Lvl7;
        }
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            AudioClip clipToPlay = sounds.Find(x => x.type == type).soundClip;
            AudioSource availableSource = GetAvailableAudioSource();
            availableSource.clip = clipToPlay;
            availableSource.Play();
            availableSource.volume = (soundVolume/2);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        AudioSource audioSource = audioSourcePool.Dequeue();

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSourcePool.Enqueue(audioSource);
        return audioSource;
    }

}