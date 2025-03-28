using System;

namespace Clickbait.Utilities
{
    public abstract class Timer
    {
        protected float _initialTime;
        protected float Time { get; set; }
        public bool IsRunning { get; protected set; }

        public float Progress => 1 - (Time / _initialTime);

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };
        public Action OnAbort = delegate { };

        protected Timer(float time)
        {
            _initialTime = time;
            IsRunning = false;
        }

        public void Start()
        {
            Time = _initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop();
            }
        }

        public void Abort()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnAbort();
            }
        }

        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;

        public abstract void Tick(float deltaTime);
    }


    public class CountDownTimer : Timer
    {
        public CountDownTimer(float initialTime) : base(initialTime) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                if (Time > 0)
                {
                    Time -= deltaTime;
                }

                if (Time <= 0)
                {
                    Time = 0;
                    Stop();
                }
            }
        }

        public bool IsFinished => Time <= 0;

        public void Reset()
        {
            Stop();
            Time = _initialTime;
        }

        public void Reset(float newTime)
        {
            _initialTime = newTime;
            Reset();
        }
    }


    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                Time += deltaTime;
            }
        }

        public void Reset()
        {
            Stop();
            Time = 0;
        }

        public void Reset(float newTime)
        {
            _initialTime = newTime;
            Reset();
        }

        public float GetTime() => Time;
    }
}