using System;
using Clickbait.Utilities;
using UnityEngine;

/// <summary>
/// If HeartRate is in the:
///  1) Chill range -> Keep decreasing room size
///  2) Panic Range -> increase room size
///  3) Borderline Panic Range -> Keep Value, But if it stays there for too long, increase room size
/// </summary>
[System.Serializable]
public class PanicOrChillStrategy
{
    [SerializeField] int[] _borderlinePanicRange;

    [SerializeField] float _initialDelay = 5;

    [Header("Panicking Timers")]
    // Give this much time to calm down - otherwise start increasing room size
    [SerializeField]
    float _borderlineDuration = 5;

    [Header("Chilling")]
    // Decrease room size for 5 second(interval), then wait for 5 seconds(cooldown) before increasing again
    [SerializeField]
    float _increaseDifficultyInterval = 5;

    [SerializeField] float _increaseDifficultyCooldown = 5;

    // If borderLineDuration passes and then heart rate calms down, wait a few seconds before decreasing room size again
    [SerializeField] float _cooldownDurationAfterPlayerRecovers = 5;

    [Header("Panicking")]
    // Increase room size for 1 second(interval), then wait for 5 seconds(cooldown) before increasing again
    [SerializeField]
    float _reduceDifficultyInterval = 2;

    [SerializeField] float _reduceDifficultyCooldown = 4;

    [SerializeField] State _state;
    [SerializeField, NonEditable] bool _stayedInBorderlineTooLong;

    IMoveValueStrategy _moveValueStrategy;

    CountDownTimer _initialDelayCountdownTimer;

    // Chill Timer
    CountDownTimer _recoveryTimer;

    // Borderline Timer
    CountDownTimer _borderlineTimer;

    // Increasing Difficulty Timers
    CountDownTimer _increaseDifficultyIntervalTimer;
    CountDownTimer _increaseDifficultyCooldownTimer;

    // Panic Timers
    CountDownTimer _reduceDifficultyIntervalTimer;
    CountDownTimer _reduceDifficultyCooldownTimer;

    public Observer<bool> IsMoving = new(false);

    enum State
    {
        Chill,
        Panic,
        Borderline
    }

    [System.Serializable]
    public struct TimersRunning
    {
        public bool InitialDelay;

        [Header("Chill")] public bool IncreaseDiffInterval;
        public bool IncreaseDiffCooldown;
        public bool RecoveryTimerCooldown;

        [Header("Borderline")] public bool BorderlineTimer;

        [Header("Panic")] public bool ReduceDiffInterval;
        public bool ReduceDiffCooldown;
    }

    [SerializeField] TimersRunning _timersRunning;

    public void Initialize(IMoveValueStrategy moveValueStrategy)
    {
        _moveValueStrategy = moveValueStrategy;

        _initialDelayCountdownTimer = new CountDownTimer(_initialDelay);
        _timersRunning.InitialDelay = true;
        _initialDelayCountdownTimer.OnTimerStop += Begin;
        _initialDelayCountdownTimer.Start();

        // Chill state
        _increaseDifficultyIntervalTimer = new CountDownTimer(_increaseDifficultyInterval);
        _increaseDifficultyCooldownTimer = new CountDownTimer(_increaseDifficultyCooldown);

        _increaseDifficultyIntervalTimer.OnTimerStart += () =>
        {
            if (_moveValueStrategy.CanDecreaseSize())
            {
                IsMoving.Value = true;
            }
        };
        
        Action increaseDiffTimer_disableIsMoving = () =>
        {
            // Set to false if the difficulty reducer isn't running either
            if (!_timersRunning.ReduceDiffInterval)
            {
                IsMoving.Value = false;
            }
        };
        
        _increaseDifficultyIntervalTimer.OnTimerStop += increaseDiffTimer_disableIsMoving;
        _increaseDifficultyIntervalTimer.OnAbort += increaseDiffTimer_disableIsMoving;

        _increaseDifficultyIntervalTimer.OnTimerStop += OnIncreaseDifficultyIntervalEnd;
        _increaseDifficultyCooldownTimer.OnTimerStop += OnIncreaseDifficultyCooldownEnd;

        _recoveryTimer = new CountDownTimer(_cooldownDurationAfterPlayerRecovers);

        // Borderline state
        _borderlineTimer = new CountDownTimer(_borderlineDuration);
        _borderlineTimer.OnTimerStop += () =>
        {
            _stayedInBorderlineTooLong = true;
            UpdateState(State.Panic, true);
        };

        // Panic state
        _reduceDifficultyIntervalTimer = new CountDownTimer(_reduceDifficultyInterval);
        _reduceDifficultyCooldownTimer = new CountDownTimer(_reduceDifficultyCooldown);

        _reduceDifficultyIntervalTimer.OnTimerStart += () =>
        {
            if (_moveValueStrategy.CanIncreaseSize())
            {
                IsMoving.Value = true;
            }
        };

        Action reduceDiffTimer_disableIsMoving = () =>
        {
            // Set to false if the difficulty increaser isn't running either
            if (!_timersRunning.IncreaseDiffInterval)
            {
                IsMoving.Value = false;
            }
        };
        _reduceDifficultyIntervalTimer.OnTimerStop += reduceDiffTimer_disableIsMoving;
        _reduceDifficultyIntervalTimer.OnAbort += reduceDiffTimer_disableIsMoving;

        _reduceDifficultyIntervalTimer.OnTimerStop += OnReduceDifficultyIntervalEnd;
        _reduceDifficultyCooldownTimer.OnTimerStop += OnReduceDifficultyCooldownEnd;
    }

    public void Update(float dt, int heartRate)
    {
        if (_initialDelayCountdownTimer.IsRunning)
        {
            _initialDelayCountdownTimer.Tick(dt);
            return;
        }

        UpdateTimers(dt);

        State newState = GetCurrentState(heartRate);
        if (newState != _state)
        {
            UpdateState(newState);
        }

        HandleStateLogic(dt, heartRate);
    }

    void Begin()
    {
        _timersRunning.InitialDelay = false;
        
        _increaseDifficultyIntervalTimer.Start();
    }

    void UpdateTimers(float dt)
    {
        _recoveryTimer.Tick(dt);

        _timersRunning.IncreaseDiffInterval = _increaseDifficultyIntervalTimer.IsRunning;
        _timersRunning.IncreaseDiffCooldown = _increaseDifficultyCooldownTimer.IsRunning;
        _timersRunning.RecoveryTimerCooldown = _recoveryTimer.IsRunning;

        _timersRunning.BorderlineTimer = _borderlineTimer.IsRunning;

        _timersRunning.ReduceDiffInterval = _reduceDifficultyIntervalTimer.IsRunning;
        _timersRunning.ReduceDiffCooldown = _reduceDifficultyCooldownTimer.IsRunning;
    }

    void HandleStateLogic(float dt, float heartRate)
    {
        switch (_state)
        {
            case State.Chill:
                if (_timersRunning.IncreaseDiffInterval)
                {
                    _increaseDifficultyIntervalTimer.Tick(dt);
                    bool canUpdate = _moveValueStrategy.TryUpdateValue(dt, false);
                    if (!canUpdate)
                    {
                        IsMoving.Value = false;
                    }
                } else
                {
                    _increaseDifficultyCooldownTimer.Tick(dt);
                }

                break;

            case State.Borderline:
                _borderlineTimer.Tick(dt);
                break;

            case State.Panic:
                if (_timersRunning.ReduceDiffInterval)
                {
                    _reduceDifficultyIntervalTimer.Tick(dt);
                    bool canUpdate = _moveValueStrategy.TryUpdateValue(dt, true);
                    if (!canUpdate)
                    {
                        IsMoving.Value = false;
                    }
                } else
                {
                    _reduceDifficultyCooldownTimer.Tick(dt);
                }

                if (_stayedInBorderlineTooLong)
                {
                    if (heartRate > _borderlinePanicRange[1])
                    {
                        _stayedInBorderlineTooLong = false;
                    }
                }

                break;
        }
    }

    void UpdateState(State newState, bool comingFromExcessBorderlineTime = false)
    {
        State oldState = _state;
        _state = newState;

        // Discard previous state logic
        switch (oldState)
        {
            case State.Chill:
                _increaseDifficultyIntervalTimer.Abort();
                _increaseDifficultyCooldownTimer.Abort();
                _recoveryTimer.Abort();
                break;

            case State.Borderline:
                _borderlineTimer.Abort();

                if (_state == State.Chill || (_state == State.Panic && !comingFromExcessBorderlineTime))
                {
                    _stayedInBorderlineTooLong = false;
                }

                break;

            case State.Panic:
                _reduceDifficultyIntervalTimer.Abort();
                _reduceDifficultyCooldownTimer.Abort();
                break;
        }

        // Implement current state logic
        switch (_state)
        {
            case State.Chill:
                _increaseDifficultyIntervalTimer.Start();
                _recoveryTimer.Start();
                _stayedInBorderlineTooLong = false;
                break;

            case State.Borderline:
                _borderlineTimer.Start();
                break;

            case State.Panic:
                _reduceDifficultyIntervalTimer.Start();
                break;
        }
    }

    State GetCurrentState(int heartRate)
    {
        State newState;

        if (heartRate < _borderlinePanicRange[0])
        {
            newState = State.Chill;
        } else if (heartRate > _borderlinePanicRange[1])
        {
            newState = State.Panic;
        } else
        {
            newState = State.Borderline;
        }

        if (_stayedInBorderlineTooLong && newState == State.Borderline)
        {
            newState = State.Panic;
        }

        return newState;
    }

    void OnIncreaseDifficultyIntervalEnd()
    {
        if (_state == State.Chill)
        {
            _increaseDifficultyCooldownTimer.Start();
        }
    }

    void OnIncreaseDifficultyCooldownEnd()
    {
        if (_state == State.Chill)
        {
            _increaseDifficultyIntervalTimer.Start();
        }
    }

    void OnReduceDifficultyIntervalEnd()
    {
        if (_state == State.Panic)
        {
            _reduceDifficultyCooldownTimer.Start();
        }
    }

    void OnReduceDifficultyCooldownEnd()
    {
        if (_state == State.Panic)
        {
            _reduceDifficultyIntervalTimer.Start();
        }
    }
}