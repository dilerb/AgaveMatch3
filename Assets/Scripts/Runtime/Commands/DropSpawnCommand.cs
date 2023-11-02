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
        
        public DropSpawnCommand(BoardData data, List<GameObject> tiles)
        {
            _boardData = data;
            _tileList = tiles;
        }

        public void Execute()
        {
            for (int i = 0; i < _tileList.Count; i++)
            {
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