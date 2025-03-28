using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] bool _followX = true;
    [SerializeField] bool _followY = true;
    [SerializeField] bool _followZ = true;
    [SerializeField] bool _followRot = true;
    
    [SerializeField] bool _followRotX = true;
    [SerializeField] bool _followRotY = true;
    [SerializeField] bool _followRotZ = true;
    
    void Update()
    {
        Vector3 targetPos = new Vector3(
            _followX ? _target.position.x : transform.position.x,
            _followY ? _target.position.y : transform.position.y,
            _followZ ? _target.position.z : transform.position.z
            );
        
        transform.position = targetPos;
        
        if (_followRot)
        {
            Vector3 targetRot = new Vector3(
                _followRotX ? _target.rotation.eulerAngles.x : transform.rotation.eulerAngles.x,
                _followRotY ? _target.rotation.eulerAngles.y : transform.rotation.eulerAngles.y,
                _followRotZ ? _target.rotation.eulerAngles.z : transform.rotation.eulerAngles.z
            );
            transform.rotation = Quaternion.Euler(targetRot);
        }
    }
}
