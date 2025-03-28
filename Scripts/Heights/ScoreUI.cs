using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] ZombieSpawner _zombieSpawner;
    [SerializeField] TextMeshProUGUI _zombiesKilledCountText;

    int _maxZombies;
    
    void Start()
    {
        _maxZombies = _zombieSpawner.MaxZombies;
        _zombieSpawner.OnZombieKilled += UpdateScoreUI;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        _zombiesKilledCountText.text = $"{_zombieSpawner.KillCount}/{_maxZombies}";
    }
}
