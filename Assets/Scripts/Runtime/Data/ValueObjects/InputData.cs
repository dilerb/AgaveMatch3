using System;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct InputData
    {
        public float HorizontalSwipeDistanceMin;
        public float VerticalSwipeDistanceMin;
        public float HorizontalSwipeDistanceMax;
        public float VerticalSwipeDistanceMax;
    }
}