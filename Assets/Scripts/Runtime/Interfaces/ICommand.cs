using Runtime.Keys;

namespace Runtime.Interfaces
{
    public interface ICommand
    {
        public void Execute() { }
        public void Execute(MatchInfoParams parameter) { }
    }
}