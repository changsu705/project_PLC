using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource MasterPlayer;
    public AudioSource BGMPlayer;
    public AudioSource SFXPlayer;
    public AudioSource FootstepPlayer;

    public AudioClip[] BGMList;
    public AudioClip[] SFXList;
    public AudioClip[] FootstepList;

    private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
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

    private void OnSceneLoaded(Scene Scene2, LoadSceneMode Scene3)
    {
        if (clips.ContainsKey(Scene2.name))
        {
            BGMPlayer.clip = clips[Scene2.name];
            BGMPlayer.Play();
        }
    }

    public void PlaySFX(string name)
    {
        SFXPlayer.PlayOneShot(clips[name]);
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