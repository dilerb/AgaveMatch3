using System.Collections.Generic;
using Runtime.Commands;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers
{
    public class MatchController: MonoBehaviour
    {
        private BoardData _boardData;
        private List<GameObject> _tileList;
        private GameObject[] _dropList;

        private List<int> _matchDropIndexListMerged;
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
            
            _matchDropIndexListMerged = new List<int>();

            if (_matchDropIndexList1.Count > 2)
            {
                _matchDropIndexListMerged.AddRange(_matchDropIndexList1);
            }
            
            if (_matchDropIndexList2.Count > 2)
            {
                _matchDropIndexListMerged.AddRange(_matchDropIndexList2);
            }
            
            CallKillDropCommand();
            
            if (_matchDropIndexListMerged.Count == 0)
            {
                ReverseSwap(ref firstDropIndex, ref secondDropIndex);
            }
        }
        
        private void ReverseSwap(ref int firstDropIndex, ref int secondDropIndex)
        {
            var reverseSwap = new SwapDropCommand(_tileList);
            reverseSwap.Execute(ref firstDropIndex, ref secondDropIndex);
            CoreGameSignals.Instance.OnMatchFailed?.Invoke();
        }
        private void CallKillDropCommand()
        {
            if (_matchDropIndexListMerged.Count <= 0) 
                return;
            
            var killDrops = new KillDropCommand(_tileList, _matchDropIndexListMerged);
            killDrops.Execute();
        }
        internal void CallDropFallCommand()
        {
            var dropFall = new DropFallCommand(_boardData, _matchDropIndexListMerged, _tileList);
            dropFall.Execute();
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
                
                    if (_matchDropIndexList1.Count  < 3)
                    {
                        _matchDropIndexList1.Clear();
                        UpperMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count  < 3)
                    {
                        _matchDropIndexList2.Clear();
                        UpperMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                    }

                    break;
                }
                case SwipeDirection.Right:
                {
                    RightMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    LeftMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);

                    if (_matchDropIndexList1.Count < 3)
                    {
                        _matchDropIndexList1.Clear();
                        UpperMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count < 3)
                    {
                        _matchDropIndexList2.Clear();
                        UpperMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        BottomMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                    }
                    
                    break;
                }
                case SwipeDirection.Up:
                {
                    UpperMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    BottomMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                
                    if (_matchDropIndexList1.Count < 3)
                    {
                        _matchDropIndexList1.Clear();
                        LeftMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        RightMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count < 3)
                    {
                        _matchDropIndexList2.Clear();
                        LeftMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        RightMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                    }

                    break;
                }
                case SwipeDirection.Bottom:
                {
                    BottomMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    UpperMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                
                    if (_matchDropIndexList1.Count < 3)
                    {
                        _matchDropIndexList1.Clear();
                        LeftMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                        RightMatchCheck(ref _matchDropIndexList1, firstDropType, firstDropIndex, firstDropBorders);
                    }

                    if (_matchDropIndexList2.Count < 3)
                    {
                        _matchDropIndexList2.Clear();
                        LeftMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                        RightMatchCheck(ref _matchDropIndexList2, secondDropType, secondDropIndex, secondDropBorders);
                    }
                    
                    break;
                }
            }
        }

        private void LeftMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            var adjacent1 = dropIndex - 1;
            var adjacent2 = dropIndex - 2;
            
            if (adjacent1 < borders.Left) 
                return;
            
            if (CheckAdjacentMatch(adjacent1, dropType))
            {
                matchDropIndexList.Add(adjacent1);

                if (adjacent2 >= borders.Left && CheckAdjacentMatch(adjacent2, dropType))
                {
                    matchDropIndexList.Add(adjacent2);
                }
            }
            
            matchDropIndexList.Add(dropIndex);
        }
        private void RightMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            var adjacent1 = dropIndex + 1;
            var adjacent2 = dropIndex + 2;

            if (adjacent1 > borders.Right) 
                return;
            
            if (CheckAdjacentMatch(adjacent1, dropType))
            {
                matchDropIndexList.Add(adjacent1);

                if (adjacent2 <= borders.Right && CheckAdjacentMatch(adjacent2, dropType))
                {
                    matchDropIndexList.Add(adjacent2);
                }
            }

            matchDropIndexList.Add(dropIndex);
        }
        private void UpperMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            var adjacent1 = dropIndex + _boardData.Width;
            var adjacent2 = dropIndex + _boardData.Width * 2;

            if (adjacent1 > borders.Top) 
                return;
            
            if (CheckAdjacentMatch(adjacent1, dropType))
            {
                matchDropIndexList.Add(adjacent1);

                if (adjacent2 <= borders.Top && CheckAdjacentMatch(adjacent2, dropType))
                {
                    matchDropIndexList.Add(adjacent2);
                }
            }

            matchDropIndexList.Add(dropIndex);
        }
        private void BottomMatchCheck(ref HashSet<int> matchDropIndexList, DropType dropType, int dropIndex, BorderIndex borders)
        {
            var adjacent1 = dropIndex - _boardData.Width;
            var adjacent2 = dropIndex - _boardData.Width * 2;

            if (adjacent1 < borders.Bottom) 
                return;
            
            if (CheckAdjacentMatch(adjacent1, dropType))
            {
                matchDropIndexList.Add(adjacent1);

                if (adjacent2 >= borders.Bottom && CheckAdjacentMatch(adjacent2, dropType))
                {
                    matchDropIndexList.Add(adjacent2);
                }
            }

            matchDropIndexList.Add(dropIndex);
        }

        private bool CheckAdjacentMatch(int adjacent, DropType dropType)
        {
            if (CheckDropExist(adjacent) && _tileList[adjacent].GetComponentInChildren<Drop>().dropType == dropType)
            {
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