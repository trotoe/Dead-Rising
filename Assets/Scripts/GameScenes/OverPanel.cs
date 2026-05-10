using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverPanel : BasePanel
{
    [SerializeField] private Text txtResult;
    [SerializeField] private Text txtMoney;
    [SerializeField] private Button btnOk;

    protected override void Init()
    {
        btnOk.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.HidePanel<OverPanel>();
            GameManager.Instance.ClearInfo();
            SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(int money,bool isWin)
    { 
        txtResult.text = isWin ? "³É¹¦" : "Ê§°Ü";
        txtMoney.text = "$" + money;

        GameDataMgr.Instance.playerData.money += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void OnShow()
    {
        base.OnShow();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
