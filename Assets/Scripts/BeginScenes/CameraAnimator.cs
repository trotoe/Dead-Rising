using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction onPlayOver;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TurnLeft(UnityAction overAction)
    { 
        animator.SetTrigger("left");
        onPlayOver = overAction;
    }

    public void TurnRight(UnityAction overAction)
    {
        animator.SetTrigger("right");
        onPlayOver = overAction;
        
    }

    public void OnPlayOver()
    {
        onPlayOver?.Invoke();
        onPlayOver = null;
    }
}
