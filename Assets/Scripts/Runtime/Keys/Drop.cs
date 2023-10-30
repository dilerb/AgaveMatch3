using Runtime.Enums;
using UnityEngine;

namespace Runtime.Keys
{
    public class Drop: MonoBehaviour
    {
        [SerializeField] public DropType dropType;

        public Drop(DropType type)
        {
            dropType = type;
        }
    }
}