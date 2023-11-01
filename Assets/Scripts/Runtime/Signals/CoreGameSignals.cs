using System.Collections.Generic;
using Runtime.Data.ValueObjects;
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
        public UnityAction <BoardData>OnBoardDataTaken = delegate {  };
        public UnityAction <List<GameObject>>OnTileListTaken = delegate {  };
        public UnityAction OnBoardCreated = delegate {  };
        public UnityAction OnMatchCompleted = delegate {  };
        public UnityAction OnMatchFailed = delegate {  };
        public UnityAction<MatchInfoParams> OnMatchInfosTaken = delegate {  };
        public UnityAction OnReset = delegate {  };
        
        //public Func<byte> onGetMatchValue = delegate { return 0; }; 
    }
}