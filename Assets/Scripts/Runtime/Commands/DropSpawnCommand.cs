using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Keys;
using Runtime.Managers;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Commands
{
    public class DropSpawnCommand: ICommand
    {
        private readonly BoardData _boardData;
        private readonly List<GameObject> _tileList;
        private readonly bool _checkSpawnerTile;
        
        public DropSpawnCommand(BoardData data, List<GameObject> tiles, bool check)
        {
            _boardData = data;
            _tileList = tiles;
            _checkSpawnerTile = check;
        }

        public void Execute()
        {
            for (int i = 0; i < _tileList.Count; i++)
            {
                if (_tileList[i].transform.childCount > 0)
                    continue;

                if(_checkSpawnerTile && !_boardData.SpawnerTileList[i % _boardData.Width])
                    continue;
                
                var typeList = EnumToList.Convert<DropType>();

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
                        ObjectPoolManager.Instance.InstantiateDrop(Vector3.zero, Quaternion.identity, (int)randomType, _tileList[i].transform);
                        break;
                    }
                }
            }
        }
        
        private bool CheckAnyMatchOnPosition(DropType type, int tilePosition)
        {
            // LEFT CHECK 
            
            if (tilePosition % _boardData.Width > 1)
            {
                if (_tileList[tilePosition - 1].transform.childCount == 0 || 
                    _tileList[tilePosition - 2].transform.childCount == 0)
                    return false;
                
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
                if (_tileList[tilePosition - _boardData.Width].transform.childCount == 0 || 
                    _tileList[tilePosition - (_boardData.Width * 2)].transform.childCount == 0)
                    return false;
                
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
            
            // RIGHT CHECK
            
            if (tilePosition % _boardData.Width < _boardData.Width - 2)
            {
                if (_tileList[tilePosition + 1].transform.childCount == 0 || 
                    _tileList[tilePosition + 2].transform.childCount == 0)
                    return false;
                
                var neighbourR1 = _tileList[tilePosition + 1].GetComponentInChildren<Drop>().dropType;
                if (type == neighbourR1)
                {
                    var neighbourR2 = _tileList[tilePosition + 2].GetComponentInChildren<Drop>().dropType;
                    if (type == neighbourR2)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}