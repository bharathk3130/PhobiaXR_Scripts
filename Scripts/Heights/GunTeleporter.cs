using UnityEngine;

public class GunTeleporter : MonoBehaviour
{
    [SerializeField] Rigidbody _gunRb;
    [SerializeField] float _minY;

    Vector3 _gunSpawnPos;
    Quaternion _gunSpawnRot;
    
    void Start()
    {
        _gunSpawnPos = _gunRb.position;
        _gunSpawnRot = _gunRb.rotation;
    }

    void Update()
    {
        if (_gunRb.position.y < _minY)
        {
            ResetGun();
        }
    }

    void ResetGun()
    {
        _gunRb.velocity = Vector3.zero;
        _gunRb.angularVelocity = Vector3.zero;
        
        _gunRb.position = _gunSpawnPos;
        _gunRb.rotation = _gunSpawnRot;
    }
}
