using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    public int maxWave;
    public int maxNum;

    private int nowWave;
    private int nowNumber;

    public List<int> monsterId;
    public int curMonsterId;

    public float spawnWaveTime;
    public float spawnMonsterTime;
    public float firstSpawnTime;

    void Start()
    {
        GameManager.Instance.AddMonsterPoint(this);
        int length = GameDataMgr.Instance.zombieInfos.Count;
        monsterId = new List<int>(length);
        for (int i = 1; i <= length; i++)
        {
            monsterId.Add(i);
        }
        Invoke("SpawnWave", firstSpawnTime);
    }

    public void SpawnWave()
    {
        curMonsterId = monsterId[Random.Range(0, monsterId.Count)];
        nowNumber = maxNum;
        SpawnMonster();
        nowWave++;
        GameManager.Instance.UpdatWave(nowWave,maxWave);
    }

    public void SpawnMonster()
    { 
        ZombieInfo info = GameDataMgr.Instance.zombieInfos[curMonsterId - 1];

        GameObject zombieObj = Instantiate(Resources.Load<GameObject>(info.res),
            this.transform.position,Quaternion.identity);
        Zombie zombie = zombieObj.GetComponent<Zombie>();
        zombie.InitInfo(info);
        nowNumber--;
        GameManager.Instance.AddZombie(zombie);
        if (nowNumber <= 0)
        {
            if (nowWave < maxWave)
            {
                Invoke("SpawnWave", spawnWaveTime);
            }
        }
        else
        { 
            Invoke("SpawnMonster", spawnMonsterTime);
        }
    }

    public bool IsFinish()
    {
        return nowWave == maxWave && nowNumber <= 0;
    }
}
