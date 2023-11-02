using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObjects;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands
{
    public class DropFallCommand: ICommand
    {
        private const float FallDuration = 0.2f;
        private readonly List<GameObject> _tileList;
        private readonly List<int> _fallPositions;
        private readonly GameObject[] _dropList;
        private readonly bool[] _spawnerData;
        private readonly short _boardWidth, _boardHeight;
        
        public DropFallCommand(BoardData data, GameObject[] dropList, List<int> fallPositions, List<GameObject> tileList)
        {
            _boardWidth = data.Width;
            _boardHeight = data.Height;
            _spawnerData = data.SpawnerTileList;
            _dropList = dropList;
            _fallPositions = fallPositions;
            _tileList = tileList;
        }

        public void Execute()
        {
            foreach (var fallPosition in _fallPositions)
            {
                for (var i = fallPosition; i < _boardWidth * _boardHeight; i+=_boardWidth)
                {
                    var upperNeighbour = fallPosition;
                    if (_tileList[upperNeighbour].transform.childCount > 0)
                        continue;
                    
                    while (true)
                    {
                        upperNeighbour +=_boardWidth;
                    
                        if (upperNeighbour < _boardWidth * _boardHeight)
                        {
                            if(_tileList[upperNeighbour].transform.childCount > 0)
                            {
                                var targetDrop = _tileList[upperNeighbour].transform.GetChild(0);
                                FallDropObject(targetDrop, upperNeighbour - _boardWidth);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            
            DOTween.Sequence().AppendInterval(FallDuration + 0.1f)
                .OnComplete(FillEmptyTiles);
            
            CoreGameSignals.Instance.OnMatchCompleted?.Invoke();
        }

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

        private void SpawnDrop(int fallPosition)
        {
            if (_spawnerData[fallPosition % _boardWidth]) // On spawner tile
            {
                ObjectPoolManager.Instance.InstantiateDrop(Vector3.zero, Quaternion.identity, -1, _tileList[fallPosition].transform);
            }
        }

        private void FallDropObject(Transform targetDrop, int fallPosition)
        {
            targetDrop.SetParent(_tileList[fallPosition].transform);
            targetDrop.DOLocalMove(Vector2.zero, FallDuration);
        }
    }
}