using System;

namespace Typeyatsu.Core
{
    public class GameResult
    {
        public TimeSpan Time { get; set; }
        public int TypeCount { get; set; }
        public int MistypeCount { get; set; }
    }
}
