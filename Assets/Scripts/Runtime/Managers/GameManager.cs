using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class GameManager: MonoBehaviour
    {
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        private void Start()
        {
            StartGame();
        }

        private void SubscribeEvents() => CoreGameSignals.Instance.OnReset += OnReset;
        private void UnSubscribeEvents() => CoreGameSignals.Instance.OnReset -= OnReset;
        private void StartGame() => CoreGameSignals.Instance.OnGameStart?.Invoke();
        private void OnReset()
        {
            
        }
    }
}