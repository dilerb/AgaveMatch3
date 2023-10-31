using Runtime.Enums;
using UnityEngine;

namespace Runtime.Keys
{
    public struct MatchInfoParams
    {
        public SwipeDirection SwipeDirection;
        public GameObject TargetDropObject;

        public MatchInfoParams(SwipeDirection direction, GameObject target)
        {
            SwipeDirection = direction;
            TargetDropObject = target;
        }
    }
}