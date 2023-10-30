using System.Collections.Generic;
using System.Linq;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Commands
{
    public class BoardCreatorCommand: ICommand
    {
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
            CreateTiles();
            SpawnDrops();

            Debug.LogWarning("Board is created");
            CoreGameSignals.Instance.onBoardCreated?.Invoke();
        }

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

        private void SpawnDrops()
        {
            for (int i = 0; i < _tileList.Count; i++)
            {
                List<DropType> typeList = EnumToList.Convert<DropType>();

                while (typeList.Count > 0)
                {
                    var randomType = typeList[Random.Range(0, typeList.Count)];

                    if (CheckAnyMatchOnPosition(randomType, i))
                    {
                        typeList.Remove(randomType);
                        Debug.Log("Match found! Retrying...");
                    }
                    else
                    {
                        // USE Object Pooling !!!

                        var spawnObject = _dropList.FirstOrDefault(item => item.GetComponent<Drop>().dropType == randomType);
                        Object.Instantiate(spawnObject, _tileList[i].transform);
                        break;
                    }
                }
            }
        }

        private bool CheckAnyMatchOnPosition(DropType type, int tilePosition)
        {
            // SCAN STARTS FROM LEFT-BOTTOM TO RIGHT-UP
            
            // LEFT CHECK 
            
            if (tilePosition % _boardData.Width > 1)
            {
                var neighbourL1 = _tileList[tilePosition - 1].GetComponentInChildren<Drop>().dropType;
                if (type == neighbourL1)
                {
                    var neighbourL2 = _tileList[tilePosition - 2].GetComponentInChildren<Drop>().dropType;
                    if (type == neighbourL2)
                    {
                        return true;
                    }
                }
            }
            
            // BOTTOM CHECK
            
            if (tilePosition > (_boardData.Width * 2) - 1)
            {
                var neighbourB1 = _tileList[tilePosition - _boardData.Width].GetComponentInChildren<Drop>().dropType;
                if (type == neighbourB1)
                {
                    var neighbourB2 = _tileList[tilePosition - (_boardData.Width * 2)].GetComponentInChildren<Drop>().dropType;
                    if (type == neighbourB2)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}