using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Keys;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Commands
{
    public class BoardCreatorCommand: ICommand
    {
        private static readonly Vector2 BoardPosition = new Vector2(-4.5f, -4.5f);
        private readonly BoardData _boardData;
        private readonly GameObject _boardHolder;
        private readonly GameObject[] _dropList;
        private readonly GameObject _tilePrefab;

        private List<GameObject> _tileList;
        
        public BoardCreatorCommand(BoardData data, GameObject holder, GameObject[] dropList, GameObject tile)
        {
            _boardData = data;
            _boardHolder = holder;
            _dropList = dropList;
            _tilePrefab = tile;
        }

        public void Execute()
        {
            CoreGameSignals.Instance.OnBoardDataTaken?.Invoke(_boardData);
            CoreGameSignals.Instance.OnDropListTaken?.Invoke(_dropList);
            
            CreateTiles();
            
            var dropSpawn = new DropSpawnCommand(_boardData, _tileList);
            dropSpawn.Execute();
            
            SetBoardPosition();
            
            Debug.Log("Board is created");
            CoreGameSignals.Instance.OnTileListTaken?.Invoke(_tileList);
            CoreGameSignals.Instance.OnBoardCreated?.Invoke();
        }

        private void SetBoardPosition() => _boardHolder.transform.localPosition = BoardPosition;

        private void CreateTiles()
        {
            _tileList = new List<GameObject>();
            for (var col = 0; col < _boardData.Height; col++)
            {
                for (var row = 0; row < _boardData.Width; row++)
                {
                    var x = col * _boardData.CellSpace;
                    var y = row * _boardData.CellSpace;
                    var tilePosition = new Vector2(y, x);
                    var spawnedTile = Object.Instantiate(_tilePrefab, tilePosition, Quaternion.identity, _boardHolder.transform);
                    _tileList.Add(spawnedTile);
                }
            }
        }
    }
}