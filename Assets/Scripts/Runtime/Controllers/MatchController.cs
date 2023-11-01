using System.Collections.Generic;
using Runtime.Commands;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Controllers
{
    public class MatchController: MonoBehaviour
    {
        private BoardData _boardData;
        private List<GameObject> _tileList;
        private GameObject[] _dropList;

        private HashSet<int> _matchDropIndexList1, _matchDropIndexList2; // avoid occurence
        internal void SetAllData(BoardData data, List<GameObject> tiles, GameObject[] dropList)
        {
            _boardData = data;
            _tileList = tiles;
            _dropList = dropList;
        }
        internal void StartMatchProcess(MatchInfoParams matchInfo)
        {
            Debug.LogWarning("Match is started.");

            _matchDropIndexList1 = new HashSet<int>();
            _matchDropIndexList2 = new HashSet<int>();
            var fallPositions = new List<short>();
            var firstDropIndex = matchInfo.TargetDropObject.transform.parent.GetSiblingIndex();
            var secondDropIndex = 0;
            
            switch (matchInfo.SwipeDirection)
            {
                case SwipeDirection.Left:
                    secondDropIndex = firstDropIndex - 1;
                    break;
                case SwipeDirection.Right:
                    secondDropIndex = firstDropIndex + 1;
                    break;
                case SwipeDirection.Up:
                    secondDropIndex = firstDropIndex + _boardData.Width;
                    break;
                case SwipeDirection.Bottom:
                    secondDropIndex = firstDropIndex - _boardData.Width;
                    break;
            }
            
            if (!CheckDropExist(secondDropIndex))
            {
                // Empty Tile!
                Debug.LogWarning("Empty Tile");
                CoreGameSignals.Instance.OnMatchFailed?.Invoke();
                return;
            }
                    
            var swapDrop = new SwapDropCommand(_tileList);
            swapDrop.Execute(ref firstDropIndex, ref secondDropIndex);

            FillMatchingDrops(firstDropIndex, secondDropIndex, matchInfo.SwipeDirection);
            
            var matchDropIndexListMerged = new List<int>();

            if (_matchDropIndexList1.Count > 2)
            {
                matchDropIndexListMerged.AddRange(_matchDropIndexList1);
            }
            
            if (_matchDropIndexList2.Count > 2)
            {
                matchDropIndexListMerged.AddRange(_matchDropIndexList2);
            }
            
            if (matchDropIndexListMerged.Count > 0)
            {
                var killDrops = new KillDropCommand(_tileList, matchDropIndexListMerged);
                killDrops.Execute();

                var dropFall = new DropFallCommand(_boardData, _dropList, fallPositions, _tileList);
                dropFall.Execute();
            }
            else
            {
                var reverseSwap = new SwapDropCommand(_tileList);
                reverseSwap.Execute(ref firstDropIndex, ref secondDropIndex);
            }
        }

        private void FillMatchingDrops(int firstDropIndex, int secondDropIndex, SwipeDirection direction)
        {
            var firstDropType = _tileList[firstDropIndex].GetComponentInChildren<Drop>().dropType;
            var secondDropType = _tileList[secondDropIndex].GetComponentInChildren<Drop>().dropType;
            
            BorderIndex firstDropBorders = GetBorders(firstDropIndex);
            BorderIndex secondDropBorders = GetBorders(secondDropIndex);

            switch (direction)
            {
                case SwipeDirection.Left:
                {
                    LeftMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    RightMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                
                    if (_matchDropIndexList1.Count == 0)
                    {
                        UpperMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count == 0)
                    {
                        UpperMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                    }

                    break;
                }
                case SwipeDirection.Right:
                {
                    RightMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    LeftMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);

                    if (_matchDropIndexList1.Count == 0)
                    {
                        UpperMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count == 0)
                    {
                        UpperMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                    }
                    
                    break;
                }
                case SwipeDirection.Up:
                {
                    UpperMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    BottomMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                
                    if (_matchDropIndexList1.Count == 0)
                    {
                        LeftMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        RightMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count == 0)
                    {
                        LeftMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        RightMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        
                    }

                    break;
                }
                case SwipeDirection.Bottom:
                {
                    BottomMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    UpperMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                
                    if (_matchDropIndexList1.Count == 0)
                    {
                        LeftMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        RightMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count == 0)
                    {
                        LeftMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        RightMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                    }
                    
                    break;
                }
            }
        }

        private void LeftMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            if (dropIndex - 1 >= borders.Left && dropIndex - 2 >= borders.Left)
            {
                var adjacents = new List<int>();

                adjacents.Add(dropIndex - 1);
                adjacents.Add(dropIndex - 2);
                
                if (CheckAdjacentMatch(ref adjacents, dropType))
                {
                    matchDropIndexList.Add(dropIndex);
                    matchDropIndexList.AddRange(adjacents);
                }
            }
        }
        private void RightMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            if (dropIndex + 1 <= borders.Right && dropIndex + 2 <= borders.Right)
            {
                var adjacents = new List<int>();

                adjacents.Add(dropIndex + 1);
                adjacents.Add(dropIndex + 2);
                
                if (CheckAdjacentMatch(ref adjacents, dropType))
                {
                    matchDropIndexList.Add(dropIndex);
                    matchDropIndexList.AddRange(adjacents);
                }
            }
        }
        private void UpperMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            if (dropIndex + _boardData.Width <= borders.Top && dropIndex + _boardData.Width * 2 <= borders.Top)
            {
                var adjacents = new List<int>();

                adjacents.Add(dropIndex + _boardData.Width);
                adjacents.Add(dropIndex + _boardData.Width * 2);
                
                if (CheckAdjacentMatch(ref adjacents, dropType))
                {
                    matchDropIndexList.Add(dropIndex);
                    matchDropIndexList.AddRange(adjacents);
                }
            }
        }
        private void BottomMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            if (dropIndex - _boardData.Width >= borders.Bottom && dropIndex - _boardData.Width * 2 >= borders.Bottom)
            {
                var adjacents = new List<int>();

                adjacents.Add(dropIndex - _boardData.Width);
                adjacents.Add(dropIndex - _boardData.Width * 2);
                
                if (CheckAdjacentMatch(ref adjacents, dropType))
                {
                    matchDropIndexList.Add(dropIndex);
                    matchDropIndexList.AddRange(adjacents);
                }
            }
        }

        private bool CheckAdjacentMatch(ref List<int> adjacents, DropType dropType)
        {
            if (CheckDropExist(adjacents[0]) && _tileList[adjacents[0]].GetComponentInChildren<Drop>().dropType == dropType)
            {
                if (CheckDropExist(adjacents[1]) && _tileList[adjacents[1]].GetComponentInChildren<Drop>().dropType == dropType)
                {
                    return true;
                }

                adjacents.Remove(adjacents[1]);
                return true;
            }

            return false;
        }

        private BorderIndex GetBorders(int dropIndex)
        {
            var row = dropIndex / _boardData.Width;
            var leftBorderIndex = row * _boardData.Width;
            
            var nextRow = row + 1;
            var rightBorderIndex = (nextRow * _boardData.Width) - 1;
            
            var column = dropIndex % _boardData.Width;
            var bottomBorderIndex = column;
            var topBorderIndex = (_boardData.Height - 1) * _boardData.Width + column;
            
            return new BorderIndex(leftBorderIndex, rightBorderIndex, topBorderIndex, bottomBorderIndex);
        }

        private bool CheckDropExist(int dropIndex)
        {
            return _tileList[dropIndex].transform.childCount > 0;
        }
    }
}