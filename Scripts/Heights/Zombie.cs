using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] ZombieDeathEffect _deathEffect;
    
    Vector3 _target;
    ZombieSpawner _zombieSpawner;
    HeightsSFXManager _heightsSFXManager;
    
    void Start()
    {
        _target = ZombieSpawner.GetRandomZombiePos();
    }
    
    public void Initialize(ZombieSpawner zombieSpawner, HeightsSFXManager darknessSFXManager)
    {
        _zombieSpawner = zombieSpawner;
        _heightsSFXManager = darknessSFXManager;
    }
    
    void Update()
    {
        if (Vector3.SqrMagnitude(transform.position - _target) < 0.1f)
        {
            _target = ZombieSpawner.GetRandomZombiePos();
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _target, 2 * Time.deltaTime);
        
        Vector3 direction = _target - transform.position;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
        }
    }

    public void KillSelf()
    {
        _zombieSpawner.OnZombieDie();
        _heightsSFXManager.PlayZombieDeathEffect();
        Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
