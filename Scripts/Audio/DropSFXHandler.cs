using KBCore.Refs;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DropSFXHandler : ValidatedMonoBehaviour
{
    [SerializeField, Self] XRGrabInteractable _grabInteractable;
    [SerializeField] SFXManagerBase _sfxManagerBase;
    bool _isBeingHeld;

    void Start()
    {
        _grabInteractable.selectEntered.AddListener(_ => OnGrab());
        _grabInteractable.selectExited.AddListener(_ => OnLetGo());
    }

    void OnGrab()
    {
        _isBeingHeld = true;
    }

    void OnLetGo()
    {
        _isBeingHeld = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (!_isBeingHeld)
        {
            if (col.gameObject.CompareTag("Environment") || col.gameObject.CompareTag("Ground"))
            {
                _sfxManagerBase.PlayItemDropSFX(transform.position);
            }
        }
    }
}
