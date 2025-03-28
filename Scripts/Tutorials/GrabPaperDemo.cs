using System;
using UnityEngine;

public class GrabPaperDemo : DemoBase
{
    [SerializeField] Paper _heartRatePaper;

    protected override void Awake()
    {
        base.Awake();

        if (!IsCompleted)
        {
            StartDemo();
        }
    }

    void StartDemo()
    {
        _visualsGO.SetActive(true);
        _controllerAnim.Play("RightHandGrab");
        
        Action onPaperGrabbed = null;
        onPaperGrabbed = () =>
        {
            _heartRatePaper.OnGrab -= onPaperGrabbed;
            Completed();
        };
        _heartRatePaper.OnGrab += onPaperGrabbed;
    }
}