using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance;
    public static GameDataMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new GameDataMgr();
            return instance;
        }
    }

    public RoleInfo currentRole;
    public ScenesData currentScene;
    public PlayerData playerData;
    public MusicData musicData;
    public List<RoleInfo> roleInfos;
    public List<ScenesData> scenesDatas;
    public List<ZombieInfo> zombieInfos;
    public List<TowerData> towerDatas;

    private GameDataMgr()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        roleInfos = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfos");
        scenesDatas = JsonMgr.Instance.LoadData<List<ScenesData>>("ScenesInfos");
        zombieInfos = JsonMgr.Instance.LoadData<List<ZombieInfo>>("ZombieInfos");
        towerDatas = JsonMgr.Instance.LoadData<List<TowerData>>("TowerData");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }   

    public void PlaySound(string name,Vector3 pos)
    {
        AudioClip clip = Resources.Load<AudioClip>("Music/" + name);
        if (clip != null && musicData.soundOn)
        {
            AudioSource.PlayClipAtPoint(clip, pos,musicData.soundVolume);
        }
    }
}
