using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    [SerializeField] private Text txtContent;
    [SerializeField] private Button btnClose;

    protected override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipsPanel>();
        });
    }

    public void SetContent(string content,Color color)
    {
        txtContent.text = content;
        txtContent.color = color;
    }
}
