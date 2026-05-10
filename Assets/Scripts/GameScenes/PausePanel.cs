using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnResume;

    protected override void Init()
    {
        //继续游戏
        btnResume.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<PausePanel>();
        });
        //设置界面
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        //返回菜单
        btnExit.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.HidePanel<PausePanel>();
            SceneManager.LoadScene("BeginScene");
        });
    }

    public override void OnShow()
    {
        base.OnShow();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void OnHide(UnityAction onHideEvent)
    {
        base.OnHide(onHideEvent);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
