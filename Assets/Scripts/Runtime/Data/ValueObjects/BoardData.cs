using System;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct BoardData
    {
        public short Width;
        public short Height;
        public int CellSpace;
        public bool[] SpawnerTileList;
    }
}