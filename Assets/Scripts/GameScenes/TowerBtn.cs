using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Text txtTitle;
    [SerializeField] private Text txtMoney;

    private Color colorMoney;
    private Color colorImg;

    void Start()
    {
        colorMoney = txtMoney.color;
        colorImg = img.color;
    }

    public void InitInfo(int id ,string inputStr)
    {
        // 检查引用
        if (img == null || txtTitle == null || txtMoney == null)
        {
            Debug.LogError($"[TowerBtn] UI 引用缺失 - img:{img != null}, txtTitle:{txtTitle != null}, txtMoney:{txtMoney != null}");
            return;
        }

        // 检查 towerDatas
        if (GameDataMgr.Instance.towerDatas == null)
        {
            Debug.LogError("[TowerBtn] towerDatas 为空");
            return;
        }

        // 检查 id 范围
        if (id < 1 || id > GameDataMgr.Instance.towerDatas.Count)
        {
            Debug.LogError($"[TowerBtn] ID 无效: {id} (有效范围: 1-{GameDataMgr.Instance.towerDatas.Count})");
            return;
        }

        TowerData data = GameDataMgr.Instance.towerDatas[id - 1];
        txtTitle.text = inputStr;

        // 加载 Sprite
        Sprite sprite = Resources.Load<Sprite>(data.imgRes);
        if (sprite == null)
        {
            Debug.LogWarning($"[TowerBtn] Sprite 未找到: {data.imgRes}");
            return;
        }
        img.sprite = sprite;
        txtMoney.text = "$" + data.money;

        // 如果颜色还没初始化，使用默认颜色
        if (colorMoney == default(Color)) colorMoney = Color.white;
        if (colorImg == default(Color)) colorImg = Color.white;

        // 使用玩家当前的局内金币判断
        int playerMoney = GameManager.Instance.player != null ? GameManager.Instance.player.GetMoney() : 0;
        bool canBuy = data.money <= playerMoney;
        txtMoney.color = canBuy ? colorMoney : Color.red;
        img.color = canBuy ? colorImg : Color.gray;

        Debug.Log($"[TowerBtn] {data.name} - ${data.money} - 拥有: ${playerMoney} - {(canBuy ? "可购买" : "金币不足")}");
    }
}
