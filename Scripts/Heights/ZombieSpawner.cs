using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Zombie _zombiePrefab;
    [SerializeField] CompletionUI _completionUI;
    [SerializeField] HeightsSFXManager _heightsSFXManager;
    
    [Header("Settings")]
    [SerializeField] int _maxZombies = 20;
    [SerializeField] int _maxZombiesAtATime = 4;
    [SerializeField] float _initialSpawnDelay = 5;
    [SerializeField] float _initialSpawnInterval = 3;
    [SerializeField] float _spawnDelay = 2;
    [SerializeField] float[] _zombieScreamIntervalRange;

    public event Action OnZombieKilled = delegate { };

    int _enemiesAlive;
    int _spawnCount;
    int _killCount;

    bool _allZombiesKilled;
    bool _allZombiesSpawned;

    public int MaxZombies => _maxZombies;
    public int KillCount => _killCount;
    
    void Start()
    {
        StartCoroutine(SpawnInitialZombies());
        StartCoroutine(PlayZombieScreams());
    }

    IEnumerator PlayZombieScreams()
    {
        float waitDuration = Random.Range(_zombieScreamIntervalRange[0], _zombieScreamIntervalRange[1]);
        yield return new WaitForSeconds(waitDuration);
        
        while (!_allZombiesKilled)
        {
            waitDuration = Random.Range(_zombieScreamIntervalRange[0], _zombieScreamIntervalRange[1]);
            _heightsSFXManager.PlayZombieScream();
            
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator SpawnInitialZombies()
    {
        yield return new WaitForSeconds(_initialSpawnDelay);
        for (int i = 0; i < _maxZombiesAtATime; i++)
        {
            if (_enemiesAlive >= _maxZombiesAtATime)
                yield break;
            
            SpawnZombie();
            yield return new WaitForSeconds(_initialSpawnInterval);
        }
    }

    public void OnZombieDie()
    {
        _killCount++;
        _enemiesAlive--;
        OnZombieKilled.Invoke();
        
        if (_killCount >= _maxZombies)
        {
            _allZombiesKilled = true;
            _completionUI.EnableSelf();
            _heightsSFXManager.PlayCompletedLevelSFX();
        }
        
        if (_enemiesAlive < _maxZombiesAtATime && _spawnCount < _maxZombies)
        {
            Invoke(nameof(SpawnZombie), _spawnDelay);
        }
    }

    void SpawnZombie()
    {
        if (_allZombiesSpawned) return;
        
        Zombie zombie = Instantiate(_zombiePrefab, GetRandomZombiePos(), Quaternion.identity);
        zombie.Initialize(this, _heightsSFXManager);
        
        _spawnCount++;
        _enemiesAlive++;

        if (_spawnCount >= _maxZombies)
        {
            _allZombiesSpawned = true;
        }
    }

    public static Vector3 GetRandomZombiePos() => new(Random.Range(-57, -18), 4.5f, Random.Range(-156, -30));
}
