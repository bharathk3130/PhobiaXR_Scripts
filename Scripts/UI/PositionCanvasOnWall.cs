using System;
using Clickbait.Utilities;
using KBCore.Refs;
using UnityEngine;

public class PositionCanvasOnWall : ValidatedMonoBehaviour
{
    [SerializeField, Scene] BreathingManager _breathingManager;
    [SerializeField] LayerMask _environmentLayerMask;
    [SerializeField] LayerMask _breathingUILayerMask;
    [SerializeField] float _distFromWall;
    [SerializeField] float _maxViewAngle = 50;
    [SerializeField] float _teleportCooldown = 5;
    
    Transform _cam;
    LayerMask _environmentAndBreathingUILayerMask;

    CountDownTimer _teleportCooldownTimer;

    void Awake()
    {
        _teleportCooldownTimer = new CountDownTimer(_teleportCooldown);
    }

    void Start()
    {
        _cam = Camera.main.transform;
        _environmentAndBreathingUILayerMask = _environmentLayerMask | _breathingUILayerMask;

        _breathingManager.OnBreathingEnabled += PositionOnWall;
    }

    void Update()
    {
        if (_teleportCooldownTimer.IsRunning)
        {
            _teleportCooldownTimer.Tick(Time.deltaTime);
        } else
        {
            if (IsOutOfView()) // || IsViewObstructed())
            {
                PositionOnWall();
            }
        }
    }

    void PositionOnWall()
    {
        if (GetHit(out RaycastHit hit))
        {
            Vector3 pos = GetPosAwayFromWall(hit.point);
            
            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(hit.normal);
            
            _teleportCooldownTimer.Start();
        }
    }

    bool GetHit(out RaycastHit hit)
    {
        return Physics.Raycast(_cam.position, _cam.forward, out hit, 100, _environmentLayerMask);
    }

    Vector3 GetPosAwayFromWall(Vector3 target)
    {
        Vector3 dir = (target - _cam.position).normalized;
        float dist = Vector3.Distance(target, _cam.position) - _distFromWall;
        return _cam.position + (dir * dist);
    }
    
    bool IsOutOfView()
    {
        Vector3 toObject = (transform.position - _cam.position).normalized;
        float angle = Vector3.Angle(_cam.forward, toObject);
        return angle > _maxViewAngle;
    }

    bool IsViewObstructed()
    {
        Vector3 dir = transform.position - _cam.position;
        if (Physics.Raycast(_cam.position, dir.normalized, out RaycastHit hit, dir.magnitude,
                _environmentAndBreathingUILayerMask, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.gameObject.layer == _breathingUILayerMask)
            {
                return true;
            }
        }

        return false;
    }
}
