using Runtime.Controllers;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class MatchManager: MonoBehaviour
    {
        [SerializeField] private MatchController matchController;
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onMatchInfosTaken += MatchInfosTaken;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onMatchInfosTaken -= MatchInfosTaken;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void MatchInfosTaken(MatchInfoParams infos)
        {
            matchController.StartMatchProcess(infos);
        }

        private void OnReset()
        {
            
        }
    }
}