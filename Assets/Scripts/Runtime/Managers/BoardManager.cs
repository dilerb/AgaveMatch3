using System.Collections.Generic;
using Mono.Cecil;
using Runtime.Commands;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class BoardManager: MonoBehaviour
    {
        private BoardCreationCommand _boardCreation;
        private BoardData _boardData;
        private GameObject[] _dropList;
        
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();

        private void Awake()
        {
            SetBoardData();
            SetDropList();
            Init();
        }
        private void SetBoardData() =>  _boardData = Resources.Load<CD_Board>($"Data/CD_Board").Data;
        private void SetDropList() => _dropList = Resources.LoadAll<GameObject>($"Prefabs/Drops");
        private void Init() => _boardCreation = new BoardCreationCommand(_boardData, this.gameObject, _dropList);
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onGameStart += CreateBoard;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onGameStart -= CreateBoard;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void CreateBoard() => _boardCreation.Execute();

        private void OnReset()
        {
            
        }
    }
}