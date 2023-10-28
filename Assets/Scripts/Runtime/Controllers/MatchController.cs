using Runtime.Keys;

namespace Runtime.Controllers
{
    public class MatchController
    {
        private MatchInfoParams _matchInfo;
        internal void StartMatchProcess(MatchInfoParams info)
        {
            _matchInfo = info;
        }
    }
}