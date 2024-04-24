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

    private void Update()
    {
        // BGM이 끝났을 때 재생을 반복하도록 처리
        if (!BGMPlayer.isPlaying && isBGMPlaying)
        {
            PlayBGM();
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

    private string currentBGMName;
    private AudioClip currentBGM;
    private bool isBGMPlaying = false;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string bgmName;

        if (scene.name == "MainScene")
        {
            bgmName = "main";
        }

        else if (scene.name == "Village" || scene.name == "HouseScene")
        {
            bgmName = "town";
        }
        else
        {
            bgmName = "chap1-1";
        }

        if (currentBGMName != bgmName)
        {
            currentBGMName = bgmName;
            currentBGM = clips[bgmName];
            PlayBGM();
        }
    }

    private void PlayBGM()
    {
        BGMPlayer.clip = currentBGM;
        BGMPlayer.Play();
        isBGMPlaying = true;
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