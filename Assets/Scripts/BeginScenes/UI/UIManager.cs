using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;
    private static bool isInitialized = false;
    public static bool IsInitialized => isInitialized;

    private Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();

    private Transform canvasTrans;

    private UIManager(){}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (!isInitialized)
        {
            GameObject canvas = Instantiate(Resources.Load<GameObject>("UI/Canvas"));
            canvasTrans = canvas.transform;
            DontDestroyOnLoad(canvas);
            DontDestroyOnLoad(this.gameObject);
            isInitialized = true;
        }
    }

    public T ShowPanel<T>() where T : BasePanel
    { 
        string panelName = typeof(T).Name;
        if(panelDict.ContainsKey(panelName))
        {
            return panelDict[panelName] as T;
        }

        GameObject panelObj = Instantiate(Resources.Load<GameObject>($"UI/{panelName}"));
        panelObj.transform.SetParent(canvasTrans,false);

        T panel = panelObj.GetComponent<T>();
        panelDict.Add(panelName, panel);
        panel.OnShow();
        return panel;
    }

    public void HidePanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDict.ContainsKey(panelName))
        {
            panelDict[panelName].OnHide(() =>
            {
                Destroy(panelDict[panelName].gameObject);
                panelDict.Remove(panelName);
            });
        }
    }

    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDict.ContainsKey(panelName))
        {
            return panelDict[panelName] as T;
        }
        return null;
    }
}
