using KBCore.Refs;
using UnityEngine;

public class ButtonClickDemo : DemoBase
{
    [SerializeField, Scene] GrabPaperDemo _grabPaperDemo; 
    [SerializeField, Scene] HRManagersHandler _hrManagersHandler;

    protected override void Awake()
    {
        base.Awake();

        _grabPaperDemo.OnComplete += StartDemo;
    }
    
    void StartDemo()
    {
        if (IsCompleted) return;
        
        _visualsGO.SetActive(true);
        _controllerAnim.Play("RightHandTrigger");
        _hrManagersHandler.OnInputTypeChange += Completed;
    }
}