using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioClip pigCatchSound;

    [SerializeField]
    private GameObject audioManagerPrefab;

    private Dictionary<Sfx, AudioClip> sfxDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        this.sfxDictionary = new Dictionary<Sfx, AudioClip>()
        {
            { Sfx.PigCatch, this.pigCatchSound }
        };
    }

    public void PlaySound(Sfx sound)
    {
        bool success = this.sfxDictionary.TryGetValue(sound, out AudioClip audioClip);

        if (success)
        {
            this.sfxSource.clip = audioClip;
            this.sfxSource.Play();
        }
    }
}

public enum Sfx
{
    PigCatch
}