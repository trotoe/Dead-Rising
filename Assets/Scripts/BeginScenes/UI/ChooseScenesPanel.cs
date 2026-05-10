using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenesPanel : BasePanel
{
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnright;
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnBack;
    [SerializeField] private Image imgScene;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtScene;

    private int currentSceneIndex = 0;
    private ScenesData currentScene;

    protected override void Init()
    {
        UpdateSceneInfo();
        //开始游戏
        btnStart.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenesPanel>();
            GameDataMgr.Instance.currentScene = currentScene;
            //加载游戏场景    
            AsyncOperation ao = SceneManager.LoadSceneAsync(currentScene.sceneName);
            ao.completed += (obj) =>
            {
                GameManager.Instance.Init();
            };
        });
        //返回上一级
        btnBack.onClick.AddListener(() => 
        {
            UIManager.Instance.HidePanel<ChooseScenesPanel>();
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        //选择场景
        btnLeft.onClick.AddListener(() =>
        {
            currentSceneIndex--;
            if (currentSceneIndex < 0)
            { 
                currentSceneIndex = GameDataMgr.Instance.scenesDatas.Count - 1;
            }
            UpdateSceneInfo();
        });

        btnright.onClick.AddListener(() =>
        {
            currentSceneIndex++;
            if (currentSceneIndex >= GameDataMgr.Instance.scenesDatas.Count)
            { 
                currentSceneIndex = 0;
            }
            UpdateSceneInfo();
        });
    }

    private void UpdateSceneInfo()
    {
        currentScene = GameDataMgr.Instance.scenesDatas[currentSceneIndex];
        ScenesData scenesData = currentScene;
        imgScene.sprite = Resources.Load<Sprite>(scenesData.imgRes);
        txtName.text = scenesData.name;
        txtScene.text = scenesData.tips;
        if (scenesData.tips == "略")
        {
            UIManager.Instance.ShowPanel<WarnningPanel>();
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            UIManager.Instance.HidePanel<WarnningPanel>();
            btnStart.gameObject.SetActive(true);
        }
    }
}
