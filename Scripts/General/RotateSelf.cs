using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    [SerializeField] float _rotSpeed = 5;

    void Update()
    {
        transform.Rotate(Vector3.up, _rotSpeed * Time.deltaTime);
    }
}
