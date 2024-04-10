using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BGMPlayer;
    public AudioSource SFXPlayer;
    public AudioSource FootstepPlayer;
    public AudioClip[] BGMList;
    public AudioClip[] SFXList;
    public AudioClip[] FootstepList;

    private void Awake()
    {
        BGMPlayer = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();
        SFXPlayer = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();
        FootstepPlayer = GameObject.Find("FootstepPlayer").GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneLoaded(Scene Scene2, LoadSceneMode Scene3)
    {
        for (int i = 0; i < BGMList.Length; i++)
        {
            if (Scene2.name == BGMList[i].name)
            {
                PlayBGM(BGMList[i]);
            }

        }
    }

    public void PlayBGM(AudioClip clip)
    {
        BGMPlayer.clip = clip;
        BGMPlayer.loop = true;
        BGMPlayer.volume = 0.3f;
        BGMPlayer.Play();
    }

    public void PlaySFX(string type)
    {
        int index = 0;
        switch(type)
        {
            case "SFX0": index = 0; break;
            case "SFX1": index = 1; break;
            case "SFX2": index = 2; break;
            case "SFX3": index = 3; break;
            case "SFX4": index = 4; break;
        }

        SFXPlayer.clip = SFXList[index];
        SFXPlayer.volume = 0.5f;
        SFXPlayer.Play();
    }
    public void PlayFootstep(string type)
    {
        int index = 0;
        switch (type)
        {
            case "Footstep1": index = 0; break;
            case "Footstep2": index = 1; break;
            case "Footstep3": index = 2; break;
        }

        FootstepPlayer.clip = FootstepList[index];
        FootstepPlayer.volume = 0.5f;
        FootstepPlayer.Play();
    }

    public void StopFootstep()
    {
        FootstepPlayer.Stop();
    }

    private void Update()   //테스트용
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.instance.PlaySFX("SFX0");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlaySFX("SFX1");
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            AudioManager.instance.PlaySFX("SFX2");
        }
    }
}