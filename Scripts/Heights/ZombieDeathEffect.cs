using UnityEngine;

public class ZombieDeathEffect : MonoBehaviour
{
    [SerializeField] GameObject _light;
    [SerializeField] float _flashDuration = 0.1f;
    
    void Start()
    {
        Invoke(nameof(DisableLight), _flashDuration);
        
        Destroy(gameObject, 2);
    }

    void DisableLight() => _light.SetActive(false);
}
