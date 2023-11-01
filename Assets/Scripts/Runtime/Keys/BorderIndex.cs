namespace Runtime.Keys
{
    public struct BorderIndex
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public BorderIndex(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}