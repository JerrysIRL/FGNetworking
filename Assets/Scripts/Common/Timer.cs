using System;
using UnityEngine;

namespace Common
{
    public class Timer
    {
        private float RemainingSeconds { get; set; }
        public event Action OnTimerEnd; 
        
        public Timer(float duration)
        {
            RemainingSeconds = duration;
        }

        public void Tick(float deltaTime)
        {
            if(RemainingSeconds == 0f)
                return;
            
            RemainingSeconds -= deltaTime;
            CheckForTimerEnd();
        }

        private void CheckForTimerEnd()
        {
            if (RemainingSeconds > 0f)
                return;
            RemainingSeconds = 0f;
            OnTimerEnd?.Invoke();
        }
    }
}