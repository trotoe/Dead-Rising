using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    private static MainTower instance;
    public static MainTower Instance => instance;

    private int hp;
    private int maxHp;
    private RoleInfo roleInfo;

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            UIManager.Instance.ShowPanel<PausePanel>();
        }
    }

    public void UpdateHp(int damage)
    {
        if (GameManager.Instance.IsGameOver) return;
        hp -= damage;
        UIManager.Instance.GetPanel<GamePanel>().UpdateHp(hp, maxHp);

        if (hp <= 0)
        {
            hp = 0;
            GameManager.Instance.IsGameOver = true;
            OverPanel deafultPanel = UIManager.Instance.ShowPanel<OverPanel>();
            deafultPanel.InitInfo((int)(GameManager.Instance.player.GetMoney() * 0.25),false);
        }
    }

    public void Init(RoleInfo roleInfo)
    { 
        this.roleInfo = roleInfo;
        this.maxHp = roleInfo.hp;
        this.hp = this.maxHp;
    }

    public int GetMaxHP()
    { 
        return maxHp;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}

