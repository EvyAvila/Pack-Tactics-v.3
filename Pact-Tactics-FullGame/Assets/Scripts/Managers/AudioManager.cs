using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    //The audio sources for various sounds in the game
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    //List to hold the audio clips
    [Header("Audio Clips")]
    public List<AudioClip> musicClips;
    public List<AudioClip> sfxClips;

    private float[] volumeLevels = { 0.0f, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };


    //Prevent the Sound Manager from being destroyed when loading a new scene
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    //Play specific music by the index
    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicClips.Count)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
        }
    }

    //Play specific sound effexts by the index
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Count)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }

    //Set the volume for the music
    public void SetMusicVolume(int volume)
    {
        musicSource.volume = volumeLevels[volume];

        //musicSource.volume = volume;
    }

    //Set the volume for the sound effects
    public void SetSFXVolume(int volume)
    {
        sfxSource.volume = volumeLevels[volume];
        //sfxSource.volume = volume;
    }

    public void SetVAVolume(int volume)
    {
        //vaSource.volume = volumeLevels[volume]; 
    }
}
