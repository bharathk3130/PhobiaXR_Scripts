using Clickbait.Utilities;
using KBCore.Refs;
using UnityEngine;

public class LiftSizeController : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] LiftSFXManager _liftSFXManager;
    [SerializeField] Transform _walls;
    [SerializeField] Transform _camTransform;

    [Header("Settings")]
    [SerializeField] float _scaleChangeSpeed = 0.1f;
    [SerializeField] Vector2 _roomMinXZSize;
    [SerializeField] Vector2 _roomMaxXZSize;
    [SerializeField] PanicOrChillStrategy _panicOrChillStrategy;

    IMoveValueStrategy _moveValueStrategy;
    
    [Header("Debug - Don't change")]
    [SerializeField, Range(0, 1)] float _scalePercent = 1f;

    float _wallsPosY;
    float _wallsScaleY;
    
    public float ScalePercent => _scalePercent;
    public bool IsMoving => _panicOrChillStrategy.IsMoving.Value;

    void Awake()
    {
        _wallsPosY = _walls.position.y;
        _wallsScaleY = _walls.localScale.y;
        
        _moveValueStrategy = new MoveValueLinearlyStrategy(_scaleChangeSpeed);
        
        _panicOrChillStrategy.Initialize(_moveValueStrategy);
        _panicOrChillStrategy.IsMoving.AddListener(OnLiftMovingToggle);
    }

    void Update()
    {
        _panicOrChillStrategy.Update(Time.deltaTime, HeartRateManager.HeartRate.Value);
        _scalePercent = _moveValueStrategy.ScalePercent;
        
        ScaleLift();
    }

    void OnLiftMovingToggle(bool isMoving)
    {
        if (isMoving)
        {
            _liftSFXManager.PlayLiftCreakingSFX();
        } else
        {
            _liftSFXManager.StopLiftCreakingSFX();
            _liftSFXManager.PlayLiftStoppedMovingSFX(transform.position);
        }
    }

    void ScaleLift()
    {
        _walls.localScale = GetScale(_scalePercent);
    }

    void ScaleAroundPlayer()
    {
        Vector3 liftScale = GetScale(_scalePercent);
        Vector3 playerToWall = _walls.position - _camTransform.position;

        Vector3 scaleFactor = new Vector3(
            liftScale.x / _walls.localScale.x,
            liftScale.y / _walls.localScale.y,
            liftScale.z / _walls.localScale.z
        );

        Vector3 scaledOffset = Vector3.Scale(playerToWall, scaleFactor);
        _walls.localScale = liftScale;

        _walls.position = (_camTransform.position + scaledOffset).With(y: _wallsPosY);
    }

    Vector3 GetScale(float scalePercent)
    {
        float scaleX = _roomMinXZSize[0] + ((_roomMaxXZSize[0] - _roomMinXZSize[0]) * scalePercent);
        float scaleZ = _roomMinXZSize[1] + ((_roomMaxXZSize[1] - _roomMinXZSize[1]) * scalePercent);
        Vector3 liftScale = new Vector3(scaleX, _wallsScaleY, scaleZ);
        
        return liftScale;
    }
}