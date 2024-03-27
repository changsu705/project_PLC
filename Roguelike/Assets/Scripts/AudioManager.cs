using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public AudioSource BGM;
    public AudioClip[] BGMList;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else if (_instance != this)
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
                BgSoundPlay(BGMList[i]);
            }

        }
    }

    public void BgSoundPlay(AudioClip clip)
    {
        BGM.clip = clip;
        BGM.loop = true;
        BGM.volume = 0.1f;
        BGM.Play();
    }
}