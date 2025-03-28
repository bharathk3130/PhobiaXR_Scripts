using System;
using UnityEngine;

public class PokeDemo : DemoBase
{
    [SerializeField] SwitchOnOff _switchOnOff;
    
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

        Action onSwitchFlip = null;
        onSwitchFlip = () =>
        {
            _switchOnOff.OnSwitchFlip -= onSwitchFlip;
            Completed();
        };
        _switchOnOff.OnSwitchFlip += onSwitchFlip;
    }
}