using UnityEngine;

public class LiftSFXManager : SFXManagerBase
{
    [SerializeField] AudioClip _liftStoppedMovingSFX;
    [SerializeField] AudioClip _wireGrabSFX;
    [SerializeField] AudioClip _wireReleaseSFX;
    [SerializeField] AudioClip _circuitBoardClearingSFX;
    [SerializeField] AudioSource _liftCreakingAudioSource;
    
    public void PlayLiftCreakingSFX()
    {
        if (!StartedSceneTransition)
        {
            _liftCreakingAudioSource.Play();
        }
    }

    public void StopLiftCreakingSFX() => _liftCreakingAudioSource.Stop();

    public void PlayLiftStoppedMovingSFX(Vector3 pos) => PlaySFX(_liftStoppedMovingSFX, pos);
    
    public void PlayWireGrabSFX(Vector3 pos) => PlaySFX(_wireGrabSFX, pos, volume: 0.1f);
    public void PlayWireDropSFX(Vector3 pos) => PlaySFX(_wireReleaseSFX, pos, volume: 0.1f);
    
    public void CircuitBoardClearingSFX(Vector3 pos) => PlaySFX(_circuitBoardClearingSFX, pos);
}