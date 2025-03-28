using KBCore.Refs;
using UnityEngine;

public class LookAtCam : ValidatedMonoBehaviour
{
    [SerializeField, Scene] Camera _mainCam;

    Transform _camTransform;

    void Awake()
    {
        _camTransform = _mainCam.transform;
    }

    void Update()
    {
        Vector3 direction = _camTransform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(-direction);
    }
}
