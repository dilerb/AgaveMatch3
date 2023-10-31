using System;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        public float MinHorizontalSwipeDistance;
        public float MinVerticalSwipeDistance;
        public float MaxHorizontalSwipeDistance;
        public float MaxVerticalSwipeDistance;
    }
}