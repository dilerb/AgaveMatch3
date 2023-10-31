using Runtime.Commands;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class BoardManager: MonoBehaviour
    {
        [SerializeField] private GameObject boardHolder;
        
        private BoardCreatorCommand _boardCreator;
        private BoardData _boardData;
        private GameObject[] _dropList;
        private GameObject _tilePrefab;
        
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();

        private void Awake()
        {
            SetBoardData();
            SetDropList();
            SetTilePrefab();
            Init();
        }
        private void SetBoardData() =>  _boardData = Resources.Load<CD_Board>($"Data/CD_Board").Data;
        private void SetDropList() => _dropList = Resources.LoadAll<GameObject>($"Prefabs/Drops");
        private void SetTilePrefab() =>  _tilePrefab = Resources.Load<GameObject>($"Prefabs/Tiles/Tile");
        private void Init() => _boardCreator = new BoardCreatorCommand(_boardData, boardHolder, _dropList, _tilePrefab);
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnGameStart += CreateBoard;
            CoreGameSignals.Instance.OnReset += OnReset;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.OnGameStart -= CreateBoard;
            CoreGameSignals.Instance.OnReset -= OnReset;
        }

        private void CreateBoard() => _boardCreator.Execute();

        private void OnReset()
        {
            
        }
    }
}