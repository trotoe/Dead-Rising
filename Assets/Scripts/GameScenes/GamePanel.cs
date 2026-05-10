using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    [SerializeField] private Button btnPause;
    [SerializeField] private Image imgHP;
    [SerializeField] private Text txtHP;
    [SerializeField] private Text txtMoney;
    [SerializeField] private Text txtWave;
    [SerializeField] private Transform botArea;
    [SerializeField] public List<TowerBtn> btnLists = new List<TowerBtn>();

    private TowerPoint towerPoint;
    private bool checkInput = false;

    protected override void Init()
    {
        btnPause.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanel<PausePanel>();
        });
    }

    public void UpdateHp(int hp,int maxHP)
    {
        txtHP.text = hp + "/" + maxHP;
        //imgHP.rectTransform.sizeDelta = new Vector2(((float)hp / maxHP) * originalHpWidth, imgHP.rectTransform.sizeDelta.y);
        imgHP.fillAmount = (float)hp / maxHP;
    }

    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }

    public void UpdateWave(int wave,int maxWave)
    {
        txtWave.text = wave + "/" + maxWave;
    }

    public void SetTower(TowerPoint towerPoint)
    {
        this.towerPoint = towerPoint;

        if (towerPoint == null)
        {
            checkInput = false;
            botArea.gameObject.SetActive(false);
            Debug.Log("[GamePanel] 隐藏按钮面板");
            return;
        }

        checkInput = true;
        botArea.gameObject.SetActive(true);

        Debug.Log($"[GamePanel] 显示按钮面板 - 按钮数量: {btnLists.Count}");

        if (towerPoint.towerData == null)
        {
            for (int i = 0; i < btnLists.Count; i++)
            {
                btnLists[i].gameObject.SetActive(true);
                btnLists[i].InitInfo(3 * i + 1, "数字键" + (i + 1));
            }
        }
        else
        {
            for (int i = 0; i < btnLists.Count; i++)
            {
                btnLists[i].gameObject.SetActive(false);
            }
            btnLists[1].gameObject.SetActive(true);
            btnLists[1].InitInfo(towerPoint.towerData.next, "空格键");
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!checkInput) return;
        if (towerPoint.towerData == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                towerPoint.CreatTower(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                towerPoint.CreatTower(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                towerPoint.CreatTower(7);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                towerPoint.CreatTower(towerPoint.towerData.next);
            }
        }
    }

}
