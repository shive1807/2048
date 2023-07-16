using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    AudioSource audioSource;

    public int muted = 0;
    [SerializeField] GameObject mutedImage;
    [SerializeField] GameObject UnMutedImage;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void ToggleAudio()
    {
        if(muted == 1)
        {
            audioSource.enabled = true;
            UnMutedImage.SetActive(true);
            mutedImage.SetActive(false);
            muted = 0;
        }
        else if(muted == 0) 
        {
            audioSource.enabled = false;
            mutedImage.SetActive(true);
            UnMutedImage.SetActive(false);
            muted = 1;
        }
    }
}
