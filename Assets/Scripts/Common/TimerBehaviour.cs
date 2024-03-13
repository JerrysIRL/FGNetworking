using System;
using Unity.Netcode;
using UnityEngine;

namespace Common
{
    public class TimerBehaviour : NetworkBehaviour
    {
        public NetworkVariable<bool> timerFinished = new NetworkVariable<bool>();
        public event Action OnTimerEnd = null;
        private Timer _timer;

        public override void OnNetworkSpawn()
        {
            timerFinished.Value = true;
        }

        public void StartTimer(float duration)
        {
            _timer = new Timer(duration);
            _timer.OnTimerEnd += HandleTimerEnd;
            timerFinished.Value = false;
            OnTimerEnd += () => timerFinished.Value = true;
        }

        private void HandleTimerEnd()
        {
            OnTimerEnd.Invoke();
        }

        private void Update()
        {
            if(!IsServer)
                return;
            _timer?.Tick(Time.deltaTime);
        }
    }
}