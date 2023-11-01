using System.Collections.Generic;
using Runtime.Controllers;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class MatchManager: MonoBehaviour
    {
        [SerializeField] private MatchController matchController;
        
        private GameObject[] _dropList;
        private BoardData _boardData;
        private List<GameObject> _tileList;
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnMatchInfosTaken += MatchInfosTaken;
            CoreGameSignals.Instance.OnBoardDataTaken += GetBoardData;
            CoreGameSignals.Instance.OnDropListTaken += GetDropList;
            CoreGameSignals.Instance.OnTileListTaken += GetTileList;
            CoreGameSignals.Instance.OnBoardCreated += SendDataToController;
            CoreGameSignals.Instance.OnReset += OnReset;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.OnMatchInfosTaken -= MatchInfosTaken;
            CoreGameSignals.Instance.OnBoardDataTaken -= GetBoardData;
            CoreGameSignals.Instance.OnDropListTaken -= GetDropList;
            CoreGameSignals.Instance.OnTileListTaken -= GetTileList;
            CoreGameSignals.Instance.OnBoardCreated -= SendDataToController;
            CoreGameSignals.Instance.OnReset -= OnReset;
        }

        private void GetTileList(List<GameObject> tiles) => _tileList = tiles;
        private void GetDropList(GameObject[] dropList) => _dropList = dropList;
        private void GetBoardData(BoardData data) => _boardData = data;
        private void SendDataToController() => matchController.SetAllData(_boardData, _tileList, _dropList);
        private void MatchInfosTaken(MatchInfoParams infos) => matchController.StartMatchProcess(infos);
        private void OnReset()
        {
            
        }
    }
}