using KBCore.Refs;
using UnityEngine;

public class GroundCutout : ValidatedMonoBehaviour
{
    [SerializeField, Self] Animator _anim;
    [SerializeField, Scene] MainMenuSFXManager _mainMenuSFXManager;
    [SerializeField] GameObject _cutoutModel;
    
    [SerializeField] float _animDuration = 0.75f;
    
    static readonly int _closeHash = Animator.StringToHash("Close");
    
    void Awake()
    {
        _cutoutModel.SetActive(false);
        _anim.enabled = false;
    }

    public void CreateOpening()
    {
        _mainMenuSFXManager.PlayPedestalHoleOpeningSFX(_cutoutModel.transform.position);
        _cutoutModel.SetActive(true);
        _anim.enabled = true;
    }

    public void CloseOpening()
    {
        _mainMenuSFXManager.PlayPedestalHoleClosingSFX(_cutoutModel.transform.position);
        _anim.SetTrigger(_closeHash);
        Invoke(nameof(DisableVisuals), _animDuration);
    }

    void DisableVisuals()
    {
        _cutoutModel.SetActive(false);
        _anim.enabled = false;
    }
}