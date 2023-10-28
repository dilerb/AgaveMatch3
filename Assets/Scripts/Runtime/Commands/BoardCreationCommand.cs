using Runtime.Data.ValueObjects;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Commands
{
    public class BoardCreationCommand: ICommand
    {
        private readonly BoardData _boardData;
        private readonly GameObject _boardHolder;
        private readonly GameObject[] _dropList;
        
        public BoardCreationCommand(BoardData data, GameObject holder, GameObject[] dropList)
        {
            _boardData = data;
            _boardHolder = holder;
            _dropList = dropList;
        }

        public void Execute()
        {
            
        }
    }
}