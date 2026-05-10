using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [SerializeField] private Button btnClose;
    [SerializeField] private Toggle togMusic;
    [SerializeField] private Toggle togSound;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    protected override void Init()
    {
        //初始化控件状态
        MusicData musicData = GameDataMgr.Instance.musicData;
        togMusic.isOn = musicData.musicOn;
        togSound.isOn = musicData.soundOn;
        sliderSound.value = musicData.soundVolume;
        sliderMusic.value = musicData.musicVolume;

        btnClose.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.SaveMusicData();

            UIManager.Instance.HidePanel<SettingPanel>();
        });
        //音乐开关
        togMusic.onValueChanged.AddListener((isOn) =>
        {
            BGM.Instance.PlayBGM(isOn);
            GameDataMgr.Instance.musicData.musicOn = isOn;
        });
        //音效开关
        togSound.onValueChanged.AddListener((isOn) =>
        {
            GameDataMgr.Instance.musicData.soundOn = isOn;
        });
        //音乐音量
        sliderMusic.onValueChanged.AddListener((value) =>
        {
            BGM.Instance.SetBGMValue(value);
            GameDataMgr.Instance.musicData.musicVolume = value;
        });
        //音效音量
        sliderSound.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.musicData.soundVolume = value;
        });
    }
}
