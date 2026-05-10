using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    private GameObject towerObj = null;

    public TowerData towerData = null;
    public List<int> towerId;

    public void CreatTower(int id)
    {
        TowerData data = GameDataMgr.Instance.towerDatas[id - 1];
        int currentMoney = GameManager.Instance.player.GetMoney();

        Debug.Log($"[TowerPoint] 尝试建造 {data.name} - 需要: ${data.money}, 拥有: ${currentMoney}");

        if (data.money > currentMoney)
        {
            Debug.LogWarning($"[TowerPoint] 金币不足 - 需要: ${data.money}, 拥有: ${currentMoney}");
            return;
        }

        GameManager.Instance.PlayerAddMoney(-data.money);
        if (towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }

        towerObj = Instantiate(Resources.Load<GameObject>(data.res), this.transform.position, Quaternion.identity);

        if (towerObj == null)
        {
            Debug.LogError($"[TowerPoint] 预制体加载失败: {data.res}");
            return;
        }

        Tower tower = towerObj.GetComponent<Tower>();
        if (tower == null)
        {
            Debug.LogError($"[TowerPoint] 预制体缺少 Tower 组件: {data.res}");
            return;
        }

        tower.Init(data);

        towerData = data;
        if (towerData.next != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().SetTower(this);
        }

        Debug.Log($"[TowerPoint] {data.name} 创建成功 - 剩余金币: ${GameManager.Instance.player.GetMoney()}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果没有塔，或者有塔且可以升级，才显示按钮
        if (towerData == null || towerData.next != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().SetTower(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.Instance.GetPanel<GamePanel>().SetTower(null);
    }
}
