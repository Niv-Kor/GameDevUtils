using System;

namespace GameDevUtils.EditorMode
{
    [Flags]
    public enum LogChannel : uint
    {
        General = 1 << 0,
        AI = 1 << 1,
        Player = 1 << 2,
        UI = 1 << 3,
        Audio = 1 << 4,
        Network = 1 << 5
    }
}