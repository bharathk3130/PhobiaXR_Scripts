using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTorchDemo : DemoBase
{
    [SerializeField] XRGrabInteractable _torchGrabInteractable;
    
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
        _torchGrabInteractable.selectEntered.AddListener(OnTorchGrab);
    }

    void OnTorchGrab(SelectEnterEventArgs arg0)
    {
        _torchGrabInteractable.selectEntered.RemoveListener(OnTorchGrab);
        Completed();
    }
}