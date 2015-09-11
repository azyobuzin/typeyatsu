using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace Typeyatsu.Server
{
    public static class PlayerQueue
    {
        public static string WaitingId;

        private static readonly object lockObj = new object();

        public static string MatchOrEnqueue(string id)
        {
            lock (lockObj)
            {
                if (WaitingId == null)
                {
                    WaitingId = id;
                    return null;
                }
                else
                {
                    var tmp = WaitingId;
                    WaitingId = null;
                    return tmp;
                }
            }
        }
    }
}