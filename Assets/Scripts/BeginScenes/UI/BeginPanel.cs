using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnAbout;
    [SerializeField] private Button btnQuit;

    protected override void Init()
    {
        //开始游戏
        btnStart.onClick.AddListener(() =>
        {
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });

            UIManager.Instance.HidePanel<BeginPanel>();
        });
        //设置
        btnSetting.onClick.AddListener(() => 
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        //关于
        btnAbout.onClick.AddListener(() =>
        {

        });
        //退出
        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public override void OnShow()
    {
        base.OnShow();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
