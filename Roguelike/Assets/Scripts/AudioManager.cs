using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BGMPlayer;
    public AudioSource SFXPlayer;
    public AudioClip[] BGMList;
    public AudioClip[] SFXList;

    private void Awake()
    {
        BGMPlayer = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();
        SFXPlayer = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();

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
                BGMPlay(BGMList[i]);
            }

        }
    }

    public void BGMPlay(AudioClip clip)
    {
        BGMPlayer.clip = clip;
        BGMPlayer.loop = true;
        BGMPlayer.volume = 0.1f;
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
        SFXPlayer.volume = 0.1f;
        SFXPlayer.Play();
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