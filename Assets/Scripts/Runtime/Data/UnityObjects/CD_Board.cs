using Runtime.Data.ValueObjects;
using UnityEngine;

namespace Runtime.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Board", menuName = "ScriptableObjects/CD_Board", order = 0)]
    public class CD_Board : ScriptableObject
    {
        public BoardData Data;
    }
}