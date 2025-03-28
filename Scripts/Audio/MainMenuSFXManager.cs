using UnityEngine;

public class MainMenuSFXManager : SFXManagerBase
{
    [SerializeField] AudioClip _pedestalHoleOpeningSFX;
    [SerializeField] AudioClip _pedestalHoleClosingSFX;
    [SerializeField] AudioClip _pedestalRisingSFX;
    [SerializeField] AudioClip _pedestalRetreatingSFX;
    [SerializeField] AudioClip _equippedWatchSFX;
    [SerializeField] AudioClip _heartRateDetectedSFX;
    
    public void PlayPedestalHoleOpeningSFX(Vector3? pos = null) => PlaySFX(_pedestalHoleOpeningSFX, pos);
    public void PlayPedestalHoleClosingSFX(Vector3? pos = null) => PlaySFX(_pedestalHoleClosingSFX, pos);
    public void PlayPedestalRisingSFX(Vector3? pos = null) => PlaySFX(_pedestalRisingSFX, pos);
    public void PlayPedestalRetreatingSFX(Vector3? pos = null) => PlaySFX(_pedestalRetreatingSFX, pos);
    
    public void PlayEquippedWatchSFX(Vector3? pos = null) => PlaySFX(_equippedWatchSFX, null);

    public void PlayHeartRateDetectedSFX(Vector3? pos = null) => PlaySFX(_heartRateDetectedSFX, pos);
}
