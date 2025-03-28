using UnityEngine;

public abstract class SFXManagerBase : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] AudioClip _buttonClickSFX;
    [SerializeField] AudioClip _sceneTransitionSFX;
    [SerializeField] AudioClip _paperFlyingSFX;
    [SerializeField] AudioClip _completedLevelSFX;
    [SerializeField] AudioClip _dropItemSFX;

    Transform _camTransform;
    public bool StartedSceneTransition;
    
    protected virtual void Awake()
    {
        _camTransform = _cam.transform;
    }
    
    protected void PlaySFX(AudioClip clip, Vector3? pos = null, float volume = 1)
    {
        if (!StartedSceneTransition && clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos ?? _camTransform.position, volume);
        }
    }
    
    public void PlayButtonClickSFX(Vector3? pos = null) => PlaySFX(_buttonClickSFX, pos, volume: 0.4f);
    public void PlaySceneTransitionSFX() => PlaySFX(_sceneTransitionSFX);
    public void PlayPaperFlyingSFX(Vector3 pos) => PlaySFX(_paperFlyingSFX, pos);
    public void PlayCompletedLevelSFX() => PlaySFX(_completedLevelSFX);
    public void PlayItemDropSFX(Vector3 pos) => PlaySFX(_dropItemSFX, pos);
}