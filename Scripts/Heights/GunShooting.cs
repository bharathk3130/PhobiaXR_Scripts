using System.Collections;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunShooting : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] HeightsSFXManager _heightSFXManager;
    [SerializeField] GameObject _laserEffects;
    [SerializeField] LayerMask _layersToIgnore;
    
    [Header("Settings")]
    [SerializeField] float _laserDuration = 0.1f;
    
    public Transform _shootPoint;
    
    HapticController.VibrationHand _currentHand;
    HapticController _hapticController; 

    void Start()
    {
        _hapticController = HapticController.Instance;
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.activated.AddListener(Shoot);
        
        _laserEffects.SetActive(false);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        _heightSFXManager.PlayEquipGunSFX(transform.position);

        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            if (controllerInteractor.CompareTag("RightController"))
            {
                _currentHand = HapticController.VibrationHand.Right;
            } else if (controllerInteractor.CompareTag("LeftController"))
            {
                _currentHand = HapticController.VibrationHand.Left;
            }
        }
    }

    void Shoot(ActivateEventArgs args)
    {
        StartCoroutine(PlayShootEffects());
        
        if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out RaycastHit hit, 500, ~_layersToIgnore))
        {
            if (hit.collider.TryGetComponent(out Zombie zombie))
            {
                zombie.KillSelf();
            }
        }
    }

    IEnumerator PlayShootEffects()
    {
        _heightSFXManager.PlayShootSFX();
        _hapticController.Vibrate(VibrationDataContainer.ShootVD, _currentHand);
        
        _laserEffects.SetActive(true);
        yield return new WaitForSeconds(_laserDuration);
        _laserEffects.SetActive(false);
    }
}
