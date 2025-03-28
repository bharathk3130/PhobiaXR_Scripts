using UnityEngine;

public class DarknessSFXManager : SFXManagerBase
{
    [SerializeField] AudioClip _mcbTripSFX;
    [SerializeField] AudioClip _switchSFX;
    [SerializeField] AudioClip _equipTorchSFX;

    public void PlayMCBTripSFX(Vector3 pos) => PlaySFX(_mcbTripSFX, pos);
    public void PlaySwitchSFX(Vector3 pos) => PlaySFX(_switchSFX, pos, volume: 0.1f);
    public void PlayEquipTorchSFX(Vector3 pos) => PlaySFX(_equipTorchSFX, pos);
}