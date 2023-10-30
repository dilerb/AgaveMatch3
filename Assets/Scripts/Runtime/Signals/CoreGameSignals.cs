using Runtime.Extensions;
using Runtime.Keys;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : SingletonMono<CoreGameSignals>
    { 
        public UnityAction onGameStart = delegate {  };
        public UnityAction onBoardDataTaken = delegate {  };
        public UnityAction onBoardCreated = delegate {  };
        public UnityAction onMatchCompleted = delegate {  };
        public UnityAction<MatchInfoParams> onMatchInfosTaken = delegate {  };
        public UnityAction onReset = delegate {  };
        
        //public Func<byte> onGetMatchValue = delegate { return 0; }; 
    }
}