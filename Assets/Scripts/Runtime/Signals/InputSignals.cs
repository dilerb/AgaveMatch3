using Runtime.Extensions;
using Runtime.Keys;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class InputSignals : SingletonMono<InputSignals>
    {
        public UnityAction onInputEnable = delegate {  };
        public UnityAction onInputDisable = delegate {  };
        public UnityAction<MatchInfoParams> onInputTaken = delegate {  }; 
    }
}