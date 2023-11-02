using System.Collections.Generic;
using DG.Tweening;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Commands
{
    public class SwapDropCommand: ICommand
    {
        private const float SwapDuration = 0.2f;
        private readonly List<GameObject> _tileList;
        
        public SwapDropCommand(List<GameObject> tileList)
        {
            _tileList = tileList;
        }

        public void Execute(ref int firstItemIndex, ref int secondItemIndex)
        {
            var firstItemParent = _tileList[firstItemIndex].transform;
            var secondItemParent = _tileList[secondItemIndex].transform;
            var firstItem = firstItemParent.GetChild(0);
            var secondItem = secondItemParent.GetChild(0);
            
            firstItem.SetParent(secondItemParent);
            secondItem.SetParent(firstItemParent);

            var temp = firstItemIndex;
            firstItemIndex = secondItemIndex;
            secondItemIndex = temp;

            firstItem.localPosition = Vector2.zero;
            secondItem.localPosition = Vector2.zero;
        }
    }
}