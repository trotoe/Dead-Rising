using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager
{
    private static GameManager instance = new GameManager();
    public static GameManager Instance => instance;

    public Player player;

    private int nowWave;
    public int maxWave;

    private List<Zombie> zombiesList = new List<Zombie>();

    private List<MonsterPoint> monsterPoints = new List<MonsterPoint>();

    private bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }


    public void Init()
    {
        isGameOver = false;
        UIManager.Instance.ShowPanel<GamePanel>();
        zombiesList.Clear();
        nowWave = 0;
        RoleInfo roleInfo = GameDataMgr.Instance.currentRole;
        //出生点
        Transform birthPoint = GameObject.Find("BirthPoint").transform;
        //创建玩家
        GameObject playerObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),
            birthPoint.position, birthPoint.rotation);
        player = playerObj.GetComponent<Player>();
        player.InitPlayerInfo(roleInfo);

        //设置相机跟随对象
        Camera.main.GetComponent<CameraMove>().SetTarget(player.transform);

        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainTower.Instance.UpdateHp(0);
    }

    public void UpdatWave(int nowWave, int maxWave)
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(nowWave, maxWave);
    }

    public bool CheckOver()
    {
        foreach (var point in monsterPoints)
        {
            if (!point.IsFinish())
            {
                return false;
            }
        }
        if (zombiesList.Count != 0) return false;
        return true;
    }

    public void PlayerAddMoney(int money)
    {
        player.AddMoney(money);
    }

    public void AddMonsterPoint(MonsterPoint point)
    {
        monsterPoints.Add(point);
    }

    public void AddZombie(Zombie zombie)
    {
        zombiesList.Add(zombie);
    }

    public void RemoveZombie(Zombie zombie)
    {
        zombiesList.Remove(zombie);
    }

    public void ClearInfo()
    {
        zombiesList.Clear();
        nowWave = 0;
        monsterPoints = new List<MonsterPoint>();
        player = null;
    }

    public Zombie FindZombie(Vector3 pos, float range)
    {
        foreach (var zombie in zombiesList)
        {
            if (!zombie.isDead && Vector3.Distance(zombie.transform.position, pos) < range)
            {
                return zombie;
            }
        }
        return null;
    }

    public List<Zombie> FindZombies(Vector3 pos, float range)
    {
        List<Zombie> lists = new List<Zombie>();
        foreach (var zombie in zombiesList)
        {
            if (!zombie.isDead && Vector3.Distance(zombie.transform.position, pos) < range)
            {
                lists.Add(zombie);
            }
        }
        return lists;
    }

}
