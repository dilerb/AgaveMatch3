using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObjects;
using Runtime.Interfaces;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands
{
    public class DropFallCommand: ICommand
    {
        private readonly List<GameObject> _tileList;
        private readonly List<short> _fallPositions;
        private readonly GameObject[] _dropList;
        private readonly bool[] _spawnerData;
        private readonly short _boardWidth, _boardHeight;
        
        public DropFallCommand(BoardData data, GameObject[] dropList, List<short> fallPositions, List<GameObject> tileList)
        {
            _boardWidth = data.Width;
            _dropList = dropList;
            _fallPositions = fallPositions;
            _tileList = tileList;
        }

        public void Execute()
        {
            CoreGameSignals.Instance.OnMatchCompleted?.Invoke();
            return;
            
            foreach (var fallPosition in _fallPositions)
            {
                var upperNeighbour = fallPosition;
                while (true)
                {
                    upperNeighbour +=_boardWidth;
                    if (upperNeighbour < _boardWidth * _boardHeight)
                    {
                        if(_tileList[upperNeighbour].transform.childCount > 0)
                        {
                            var targetDrop = _tileList[upperNeighbour].transform.GetChild(0);
                            FallDropObject(targetDrop, fallPosition);
                            break;
                        }
                    }
                    else
                    {
                        SpawnDrop(fallPosition);
                        break;
                    }
                }
            }

            FillEmptyTiles();
            //REGEN DROP LISTS

            CoreGameSignals.Instance.OnMatchCompleted?.Invoke();
        }

        // OPTIMIZATION ????
        private void FillEmptyTiles() 
        {
            for (short i = 0; i < _tileList.Count; i++)
            {
                if (_tileList[i].transform.childCount == 0)
                {
                    SpawnDrop(i);
                }
            }
        }

        private void SpawnDrop(short fallPosition)
        {
            if (_spawnerData[fallPosition % _boardWidth]) // On spawner tile
            {
                var spawnObject = _dropList[Random.Range(0, _dropList.Length)];
                Object.Instantiate(spawnObject, _tileList[fallPosition].transform);

                // do fall ???
            }
        }

        private void FallDropObject(Transform targetDrop, short fallPosition)
        {
            targetDrop.SetParent(_tileList[fallPosition].transform);
            targetDrop.DOLocalMove(Vector2.zero, 0.2f);
        }
    }
}