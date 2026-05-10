using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarnningPanel : BasePanel
{
    [SerializeField] private Text txtContent;

    protected override void Init()
    {
        txtContent.text = "¾´ÇëÆÚ´ý";
    }

    public void SetContent(string content, Color color)
    {
        txtContent.text = content;
        txtContent.color = color;
    }
}
