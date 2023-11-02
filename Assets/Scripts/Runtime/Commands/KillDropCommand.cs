using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands
{
    public class KillDropCommand: ICommand
    {
        private const float KillDuration = 0.2f;
        private const float KillScaleFactor = 2f;
        
        private readonly List<GameObject> _tileList;
        private readonly List<int> _matchedDrops;
        
        public KillDropCommand(List<GameObject> tileList, List<int> matchedDrops)
        {
            _tileList = tileList;
            _matchedDrops = matchedDrops;
        }

        public void Execute()
        {
            foreach (var dropIndex in _matchedDrops)
            {
                var drop = _tileList[dropIndex].transform.GetChild(0);
    
                drop.DOScale(drop.localScale / KillScaleFactor, KillDuration)
                    .OnComplete(() =>
                    {
                        ObjectPoolManager.Instance.DestroyDrop(drop.gameObject);
                    });
            }
            
            DOTween.Sequence().AppendInterval(KillDuration + 0.1f)
                .OnComplete(() => MatchSignals.Instance.OnDropsKilled?.Invoke());
        }
    }
}