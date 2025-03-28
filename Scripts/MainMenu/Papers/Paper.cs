using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Paper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SFXManagerBase _sfxManager; 
    [SerializeField] Transform _paperModel;
    [SerializeField] Transform _leftHandAttachPoint;
    [SerializeField] Transform _rightHandAttackPoint;

    float _speed = 5;

    public event Action OnGrab = delegate { };
    
    XRGrabInteractable _grabInteractable;
    
    Vector3 _paperInitialPos;
    Quaternion _paperInitialRot;

    bool _goBack;
    
    void Start()
    {
        _paperInitialPos = _paperModel.position;    
        _paperInitialRot = _paperModel.rotation;
        _grabInteractable = _paperModel.GetComponent<XRGrabInteractable>();
        
        _grabInteractable.hoverEntered.AddListener(OnHover);
    }

    void OnHover(HoverEnterEventArgs args)
    {
        IXRHoverInteractor interactor = args.interactorObject;
        if (interactor.transform.CompareTag("RightController"))
        {
            _grabInteractable.attachTransform = _rightHandAttackPoint;
        }
        else if (interactor.transform.CompareTag("LeftController"))
        {
            _grabInteractable.attachTransform = _leftHandAttachPoint;
        }
    }

    void Update()
    {
        if (_goBack)
        {
            _paperModel.position = Vector3.Lerp(_paperModel.position, _paperInitialPos, _speed * Time.deltaTime);
            _paperModel.rotation = Quaternion.Slerp(_paperModel.rotation, _paperInitialRot, _speed * Time.deltaTime);

            if (Vector3.Distance(_paperModel.position, _paperInitialPos) < 0.01f && 
                Quaternion.Angle(_paperModel.rotation, _paperInitialRot) < 1f)
            {
                _paperModel.position = _paperInitialPos;
                _paperModel.rotation = _paperInitialRot;
                
                _goBack = false;
            }
        }
    }

    public void OnPaperGrab()
    {
        _goBack = false;
        _sfxManager.PlayPaperFlyingSFX(transform.position);
        OnGrab.Invoke();
    }

    public void OnPaperDrop()
    {
        _goBack = true;
        _sfxManager.PlayPaperFlyingSFX(transform.position);
    }
}
