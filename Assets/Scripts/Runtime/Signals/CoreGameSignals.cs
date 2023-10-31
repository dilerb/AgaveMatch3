using Runtime.Extensions;
using Runtime.Keys;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : SingletonMono<CoreGameSignals>
    { 
        public UnityAction OnGameStart = delegate {  };
        public UnityAction <GameObject[]>OnDropListTaken = delegate {  };
        public UnityAction OnBoardCreated = delegate {  };
        public UnityAction OnMatchCompleted = delegate {  };
        public UnityAction<MatchInfoParams> OnMatchInfosTaken = delegate {  };
        public UnityAction OnReset = delegate {  };
        
        //public Func<byte> onGetMatchValue = delegate { return 0; }; 
    }
}