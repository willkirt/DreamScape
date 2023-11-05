using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    public AudioSource musicSource;
    public AudioSource musicSource2;
    public AudioSource sfxSource;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name, AudioSource source)
    {
        Sound s = Array.Find(musicSounds, ms => ms.clipName == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            source.clip = s.clip;
            source.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, ss => ss.clipName == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
