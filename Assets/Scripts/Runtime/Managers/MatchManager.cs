using Runtime.Controllers;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class MatchManager: MonoBehaviour
    {
        [SerializeField] private MatchController matchController;
        private GameObject[] _dropList;
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnMatchInfosTaken += MatchInfosTaken;
            CoreGameSignals.Instance.OnDropListTaken += GetDropList;
            CoreGameSignals.Instance.OnReset += OnReset;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.OnMatchInfosTaken -= MatchInfosTaken;
            CoreGameSignals.Instance.OnDropListTaken -= GetDropList;
            CoreGameSignals.Instance.OnReset -= OnReset;
        }

        private void GetDropList(GameObject[] dropList)
        {
            _dropList = dropList;
        }

        private void MatchInfosTaken(MatchInfoParams infos)
        {
            matchController.StartMatchProcess(infos, _dropList);
        }
        private void OnReset()
        {
            
        }
    }
}