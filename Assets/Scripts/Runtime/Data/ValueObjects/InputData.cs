using System;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        public float HorizontalSwipeDistance;
        public float VerticalSwipeDistance;
        public float MaxHorizontalSwipeDistance;
        public float MaxVerticalSwipeDistance;
    }
}