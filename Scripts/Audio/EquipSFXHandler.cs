using KBCore.Refs;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipSFXHandler : ValidatedMonoBehaviour
{
    [SerializeField, Scene] DarknessSFXManager _darknessSFXManager;

    void Start()
    {
        GetComponent<XRGrabInteractable>().selectEntered.AddListener(_ => 
            _darknessSFXManager.PlayEquipTorchSFX(transform.position));
    }
}