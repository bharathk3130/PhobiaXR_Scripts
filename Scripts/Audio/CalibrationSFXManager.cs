using UnityEngine;

public class CalibrationSFXManager : SFXManagerBase
{
    [SerializeField] AudioClip _buttonHoverSFX;
    [SerializeField] AudioClip _leverStopSFX;
    [SerializeField] AudioSource _leverMovingAudioSource;
    
    public void PlayButtonHoverSFX() => PlaySFX(_buttonHoverSFX, volume: 0.3f);
    public void PlayLeverStopSFX() => PlaySFX(_leverStopSFX);
    
    public void PlayLeverMovingSFX() => _leverMovingAudioSource.Play();

    public void StopLeverMovingSFX() => _leverMovingAudioSource.Stop();
}
