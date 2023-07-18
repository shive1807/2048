using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("References")]
    public AudioSource soundSource, musicSource;
    public Sound[] soundEffects, musics;

    [Header("SoundEffects")]
    [HideInInspector] public int muteSound;
    [SerializeField] GameObject mutedImage;
    [SerializeField] GameObject UnMutedImage;

    [Header("Music")]
    [HideInInspector] public int muteMusic;
    [SerializeField] GameObject musicMutedImage;
    [SerializeField] GameObject musicUnMutedImage;
    private void Start()
    {
        StartCoroutine(start());
    }

    private void starting()
    {
        muteSound = GameManager.Instance.gameData.SoundPref;
        muteMusic = GameManager.Instance.gameData.MusicPref;
        ToggleSound();
        //ToggleMusic();
    }

    IEnumerator start()
    {
        yield return new WaitForSeconds(1);
        starting();
    }
    public void ToggleSound()
    {
        if(muteSound == 1)
        {
            soundSource.mute = true;
            UnMutedImage.SetActive(true);
            mutedImage.SetActive(false);
            muteSound = 0;
        }
        else if(muteSound == 0) 
        {
            soundSource.mute = false;
            mutedImage.SetActive(true);
            UnMutedImage.SetActive(false);
            muteSound = 1;
        }
        Debug.Log(muteSound);
        SaveSystem.SaveGame(-1, false, null, -1, muteSound);
    }
    public void ToggleMusic()
    {
        if(muteMusic == 1)
        {
            musicSource.mute = false;
            musicUnMutedImage.SetActive(true);
            musicMutedImage.SetActive(false);
            muteMusic = 0;
        }
        else if(muteMusic == 0)
        {
            musicSource.mute = true;
            musicUnMutedImage.SetActive(false);
            musicMutedImage.SetActive(true);
            muteMusic = 1;
        }
        SaveSystem.SaveGame(-1, false, null, -1, -1, muteMusic);
    }
    public void PlaySound(string sound)
    {
        Sound s = Array.Find(soundEffects, x => x.name == sound);
        Play(s);
    }
    public void PlayMusic(string music)
    {
        Sound s = Array.Find(musics, x => x.name == music);
        Play(s);
    }
    private void Play(Sound s)
    {
        if (s != null)
        {
            soundSource.clip = s.clip;
            soundSource.Play();
        }
        else
        {
            Debug.LogError("sound not found");
        }
    }
}
