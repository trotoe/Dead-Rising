using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnBuy;
    [SerializeField] private Text txtRoleName;
    [SerializeField] private Text txtMoneyNumber;
    [SerializeField] private Text txtUnLock;

    private PlayerData playerData;
    private Transform rolePos;
    private GameObject currentRole;
    private RoleInfo currentRoleInfo;
    private int currentIndex = 0;   

    protected override void Init()
    {
        rolePos = GameObject.Find("RolePos").transform;
        txtMoneyNumber.text = GameDataMgr.Instance.playerData.money.ToString();

        //ÏÂÒ»¼¶³¡¾°
        btnStart.onClick.AddListener(()=>
        { 
            GameDataMgr.Instance.currentRole = currentRoleInfo;
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenesPanel>();
        });
        //ÏòÓÒÇÐ»»½ÇÉ«
        btnRight.onClick.AddListener(() =>
        {
            currentIndex++;
            if (currentIndex >= GameDataMgr.Instance.roleInfos.Count)
            { 
                currentIndex = 0;
            }
            UpdateRoleInfo();
            UpdateBtn();
        });
        //Ïò×óÇÐ»»½ÇÉ«
        btnLeft.onClick.AddListener(() =>
        {
            currentIndex--;
            if (currentIndex < 0)
            { 
                currentIndex = GameDataMgr.Instance.roleInfos.Count - 1;
            }
            UpdateRoleInfo();
            UpdateBtn();
        });
        //·µ»Ø
        btnBack.onClick.AddListener(() =>
        {
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() => 
            { 
                UIManager.Instance.ShowPanel<BeginPanel>();
            });

            UIManager.Instance.HidePanel<ChooseHeroPanel>();
        });

        btnBuy.onClick.AddListener(() =>
        {
            PlayerData playerData = GameDataMgr.Instance.playerData;
            if (playerData.money > currentRoleInfo.lockMoney)
            { 
                playerData.money -= currentRoleInfo.lockMoney;  
                txtMoneyNumber.text = playerData.money.ToString();
                playerData.lockedRole.Add(currentRoleInfo.id);
                GameDataMgr.Instance.SavePlayerData();
                Debug.Log("¹ºÂò³É¹¦");
                UIManager.Instance.ShowPanel<TipsPanel>().SetContent("¹ºÂò³É¹¦", Color.yellow);
            }
            else
            { 
                Debug.Log("¹ºÂòÊ§°Ü");
                UIManager.Instance.ShowPanel<TipsPanel>().SetContent("¹ºÂòÊ§°Ü", Color.red);
            }
            UpdateBtn();
        });
        UpdateRoleInfo();
        UpdateBtn();
    }

    private void UpdateRoleInfo()
    {
        if (currentRole != null)
        { 
            Destroy(currentRole);
            currentRole = null;
        }
        currentRoleInfo = GameDataMgr.Instance.roleInfos[currentIndex];
        currentRole = Instantiate(Resources.Load<GameObject>(currentRoleInfo.res), 
            rolePos.position, rolePos.rotation);
        Destroy(currentRole.GetComponent<Player>());
        txtRoleName.text = currentRoleInfo.tips;
    }

    private void UpdateBtn()
    {
        if (currentRoleInfo.lockMoney > 0 
            && !GameDataMgr.Instance.playerData.lockedRole.Contains(currentRoleInfo.id))
        { 
            btnStart.gameObject.SetActive(false);
            txtUnLock.text = "$" +  currentRoleInfo.lockMoney;
            btnBuy.gameObject.SetActive(true);
        }
        else
        { 
            btnStart.gameObject.SetActive(true);
            btnBuy.gameObject.SetActive(false);
        }
    }

    public override void OnHide(UnityAction callBack)
    {
        base.OnHide(callBack);
        if (currentRole != null)
        { 
            DestroyImmediate(currentRole);
            currentRole = null;
        }
    }
}
