using System;
using Clickbait.Utilities;
using KBCore.Refs;
using UnityEngine;

public class Connection : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Child] WireTip _wireTip;
    [SerializeField, Child] ConnectingWire _connectingWire; 
    [SerializeField] Transform _socket;
    [SerializeField] LineRenderer _wireLineRenderer;

    [Header("Settings")]
    [SerializeField] float _socketRadius = 0.02f;
    
    [Header("Debugging")]
    [SerializeField, NonEditable] bool _isConnected;
    
    CircuitBoard _circuitBoard;
    Transform _wireTipTransform;
    LiftSFXManager _liftSFXManager;

    public event Action OnConnect = delegate { };
    
    public bool IsConnected => _isConnected;
    public bool IsTutorial;
    
    void Start()
    {
        _wireTipTransform = _wireTip.transform;
    }

    public void Initialize(Vector3 socketPos, Material wireMat, Material socketMat)
    {
        _wireLineRenderer.material = wireMat;
        _socket.position = socketPos;
        _connectingWire.GetComponent<LineRenderer>().material = wireMat;
        _socket.GetComponent<SpriteRenderer>().material = socketMat;
    }

    void SetIsConnected(bool isConnected)
    {
        OnConnect.Invoke();
        _isConnected = isConnected;
        
        if (!IsTutorial)
        {
            _circuitBoard.OnConnectionChange.Invoke();
        }
    }

    public void OnWireGrab()
    {
        if (_isConnected)
        {
            SetIsConnected(false);
        }
        
        _liftSFXManager.PlayWireGrabSFX(_wireTip.transform.position);
    }

    public void OnWireRelease()
    {
        if (_wireTipTransform == null || _socket == null)
            return;
        
        if (Vector3.Distance(_wireTipTransform.position, _socket.position) < _socketRadius)
        {
            _wireTip.Connect();
            SetIsConnected(true);
        }
        
        _liftSFXManager.PlayWireDropSFX(_wireTip.transform.position);
    }

    public void SetCircuitBoard(CircuitBoard circuitBoard)
    {
        _circuitBoard = circuitBoard;
        _liftSFXManager = _circuitBoard.LiftSoundManager;
    }
}
