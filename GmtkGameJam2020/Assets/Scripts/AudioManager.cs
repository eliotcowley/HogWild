using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<Sfx, AudioClip> sfxDictionary;

    private void Awake()
    {
        Instance = this;

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