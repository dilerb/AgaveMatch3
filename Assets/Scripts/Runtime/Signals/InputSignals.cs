using Runtime.Extensions;
using Runtime.Keys;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class InputSignals : SingletonMono<InputSignals>
    {
        public UnityAction OnInputEnable = delegate {  };
        public UnityAction OnInputDisable = delegate {  };
        public UnityAction<MatchInfoParams> OnInputTaken = delegate {  }; 
    }
}