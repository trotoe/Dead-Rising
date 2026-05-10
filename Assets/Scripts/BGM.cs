using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM instance;
    public static BGM Instance => instance;
 
    private AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;           
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        MusicData musicData = GameDataMgr.Instance.musicData; 
        PlayBGM(musicData.musicOn);
        SetBGMValue(musicData.musicVolume);
    }

    public void PlayBGM(bool isOn)
    {
        audioSource.mute = !isOn;
    }

    public void SetBGMValue(float v)
    { 
        audioSource.volume = v;
    }
}
