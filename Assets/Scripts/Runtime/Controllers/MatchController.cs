using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers
{
    public class MatchController: MonoBehaviour
    {
        private MatchInfoParams _matchInfo;
        internal void StartMatchProcess(MatchInfoParams info)
        {
            _matchInfo = info;
            Debug.LogWarning("Match is started.");
            
            // do MATCH
            // ..
            // ..

            
            CoreGameSignals.Instance.onMatchCompleted?.Invoke();
        }
    }
}