using System;
using Unity.Netcode;
using UnityEngine;

namespace Common
{
    public class TimerBehaviour : NetworkBehaviour
    {
        public NetworkVariable<bool> timerFinished = new NetworkVariable<bool>();
        public event Action OnTimerEnd = null;
        private Timer _timer = new ();

        public override void OnNetworkSpawn()
        {
            timerFinished.Value = true;
            _timer.OnTimerEnd += HandleTimerEnd;
            OnTimerEnd += () => timerFinished.Value = true;
        }

        public void StartTimer(float duration)
        {
            _timer.StartTimer(duration);
            timerFinished.Value = false;
        }

        private void HandleTimerEnd()
        {
            OnTimerEnd?.Invoke();
        }

        private void Update()
        {
            if(!IsServer)
                return;
            _timer?.Tick(Time.deltaTime);
        }

        public override void OnNetworkDespawn()
        {
            _timer.OnTimerEnd -= HandleTimerEnd;
        }
    }
}