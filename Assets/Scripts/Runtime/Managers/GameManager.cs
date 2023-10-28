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

        private void SubscribeEvents() => CoreGameSignals.Instance.onReset += OnReset;
        private void UnSubscribeEvents() => CoreGameSignals.Instance.onReset -= OnReset;
        private void StartGame() => CoreGameSignals.Instance.onGameStart?.Invoke();
        private void OnReset()
        {
            
        }
    }
}