using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers
{
    public class MatchController: MonoBehaviour
    {
        internal void StartMatchProcess(MatchInfoParams matchInfo, GameObject[] dropList)
        {
            Debug.LogWarning("Match is started.");
            
            // do MATCH
            // ..
            // ..
            
            CoreGameSignals.Instance.OnMatchCompleted?.Invoke();
        }
    }
}