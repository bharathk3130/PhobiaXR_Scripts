using System;
using UnityEngine;

public class MoveLeverDemo : DemoBase
{
    [SerializeField] LeverCalibration _leverCalibration;
    
    bool _demoIsGoingOn;
    
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
        _demoIsGoingOn = true;
    }

    void Update()
    {
        if (_demoIsGoingOn)
        {
            if (_leverCalibration.IsGrabbed && Mathf.Abs(_leverCalibration.LeverPercent) > 0.1f)
            {
                Completed();
                _demoIsGoingOn = false;
            }
        }
    }
}