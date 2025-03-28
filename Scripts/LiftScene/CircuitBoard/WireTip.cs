using Clickbait.Utilities;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireTip : MonoBehaviour
{
    [SerializeField] Connection _connection;
    [SerializeField] Transform _socket;
    [SerializeField, NonEditable] HapticController.VibrationHand _currentHand;

    HapticController _hapticController;
    Transform _wireSocketPair;

    void Start()
    {
        _hapticController = HapticController.Instance;
        _wireSocketPair = _connection.transform;
        
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnLetGo);
    }

    void Update()
    {
        RestrictMovement();
    }

    void RestrictMovement()
    {
        transform.position = GetProjectedPosition();
    }
    
    public Vector3 GetProjectedPosition()
    {
        if (_wireSocketPair == null) // Happens when the wire pair instance gets destroyed
            return default;
        
        // Convert to local space of the socket pair
        Vector3 localPos = _wireSocketPair.InverseTransformPoint(transform.position);

        // Force Z to 0 to keep it on the local XY plane
        localPos.z = 0;
        
        // Convert back to world space
        Vector3 globalPos = _wireSocketPair.TransformPoint(localPos);
        return globalPos;
    }

    public void Connect()
    {
        transform.position = _socket.position;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
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
        
        _connection.OnWireGrab();
        _hapticController.Vibrate(VibrationDataContainer.DrawingVD, _currentHand);
    }

    void OnLetGo(SelectExitEventArgs args)
    {
        RestrictMovement();
        _connection.OnWireRelease();
        
        _hapticController.Vibrate(VibrationDataContainer.NoVibrationVD, _currentHand);
    }
}