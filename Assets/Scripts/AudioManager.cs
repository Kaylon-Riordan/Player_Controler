using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip jumpClip;
    public AudioClip footstepClip;
    public AudioClip dashClip;
    public AudioClip attackClip;
    public AudioClip backgroundMusic;

    private AudioSource soundEffectSource;
    private AudioSource backgroundMusicSource;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundEffectSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();

        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayJumpSound()
    {
        soundEffectSource.PlayOneShot(jumpClip);
    }
    public void PlayDashSound()
    {
        soundEffectSource.PlayOneShot(dashClip);
    }
    public void PlayFootstepSound()
    {
        soundEffectSource.PlayOneShot(footstepClip);
    }
    public void PlayAttackSound()
    {
        soundEffectSource.PlayOneShot(attackClip);
    }


    public void PlayBackgroundMusic()
    {
        if(!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }
    public void PauseBackgroundMusic()
    {
        backgroundMusicSource.Pause();
    }
    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }
    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
