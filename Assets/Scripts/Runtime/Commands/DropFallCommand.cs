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
        private readonly BoardData _data;
        private readonly List<GameObject> _tileList;
        private readonly List<int> _fallPositions;
        private readonly short _boardWidth, _boardHeight;
        
        public DropFallCommand(BoardData data, List<int> fallPositions, List<GameObject> tileList)
        {
            _data = data;
            _boardWidth = _data.Width;
            _boardHeight = _data.Height;
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
            var dropSpawn = new DropSpawnCommand(_data, _tileList, true);
            dropSpawn.Execute();
        }

        private void FallDropObject(Transform targetDrop, int fallPosition)
        {
            targetDrop.SetParent(_tileList[fallPosition].transform);
            targetDrop.DOLocalMove(Vector2.zero, FallDuration);
        }
    }
}