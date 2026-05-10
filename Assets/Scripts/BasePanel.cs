using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private UnityAction onHideCallBack;

    private float fadeSpeed = 0f;

    private bool isShowing = true;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = this.AddComponent<CanvasGroup>();
        }
    }
   
    protected virtual void Start()
    {
        fadeSpeed = 5f;
        Init();
    }

    protected virtual void Update()
    {
        print("Invoke");
        //淡入
        if (isShowing && canvasGroup.alpha != 1f)
        {
            canvasGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
            if(canvasGroup.alpha >= 1f)
            {
                canvasGroup.alpha = 1f;
            }
        }
        //淡出
        else if (!isShowing && canvasGroup.alpha != 0f)
        {
            canvasGroup.alpha -= fadeSpeed * Time.unscaledDeltaTime;
            if(canvasGroup.alpha <= 0f)
            {
                canvasGroup.alpha = 0f;
                onHideCallBack?.Invoke();
            }
        }
    }

    public virtual void OnShow()
    {
        canvasGroup.alpha = 0f;
        isShowing = true;
    }
    
    public virtual void OnHide( UnityAction callBack )
    {
        canvasGroup.alpha = 1f;
        isShowing = false;

        onHideCallBack = callBack;    
    }

    protected abstract void Init();

}
