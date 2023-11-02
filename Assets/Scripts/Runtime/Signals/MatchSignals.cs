using Runtime.Extensions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class MatchSignals : SingletonMono<MatchSignals>
    {
        public UnityAction OnDropsKilled = delegate {  };
        //public UnityAction<MatchInfoParams> On... = delegate {  }; 
    }
}