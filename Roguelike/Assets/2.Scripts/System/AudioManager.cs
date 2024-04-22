using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("#1. Master Player ======")]
    public AudioSource MasterPlayer;
    public float masterVolume;
    
    [Space(10)]
    
    [Header("#2. BGM Player ======")]
    public AudioSource BGMPlayer;
    public AudioClip[] BGMList;
    public float bgmVolume;
        
    [Space(10)]
        
    [Header("#3. SFX Player ======")]
    public AudioSource SFXPlayer; 
    public AudioClip[] SFXList;
    public float sfxVolume;
    
    [Space(10)]
    
    [Header("#4. Footstep Player ======")]
    public AudioSource FootstepPlayer;
    public AudioClip[] FootstepList;
    
   
    

    private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        Init();
        
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (var clip in BGMList)
        {
            clips.Add(clip.name, clip);
        }

        foreach (var clip in SFXList)
        {
            clips.Add(clip.name, clip);
        }

        foreach (var clip in FootstepList) 
        {
            clips.Add(clip.name, clip);
        }
    }

    private void Init()
    {
        BGMPlayer.volume = 0.3f;
        
        SFXPlayer.volume = 0.5f;
        
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        MasterPlayer.volume = masterVolume;
    }
    
    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        BGMPlayer.volume = bgmVolume;
    }
    
    public float GetBgmVolume()
    {
        return bgmVolume;
    }
    
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        SFXPlayer.volume = sfxVolume;
    }
    
    public float GetSfxVolume()
    {
        return sfxVolume;
    }
    

    private void OnSceneLoaded(Scene Scene2, LoadSceneMode Scene3)
    {
        if (Scene2.name == "Village")
        {
            BGMPlayer.clip = clips["town"];
            BGMPlayer.Play();
        }
        else
        {
            BGMPlayer.clip = clips["chap1"];
            BGMPlayer.Play();
        }
    }

    public void PlaySFX(string name)
    {
        SFXPlayer.PlayOneShot(clips[name]);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXPlayer.PlayOneShot(clip);
    }
    
    public void Footstep(bool play)
    {
        if (play)
        {
            if (!FootstepPlayer.isPlaying)
            {
                FootstepPlayer.Play();
            }
        }
        else
        {
            if (FootstepPlayer.isPlaying)
            {
                FootstepPlayer.Stop();
            }
        }
    }
}