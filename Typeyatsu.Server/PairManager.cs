using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace Typeyatsu.Server
{
    public static class PairManager
    {
        private static readonly ConcurrentDictionary<string, PlayerModel> pairs = new ConcurrentDictionary<string, PlayerModel>();

        public static void AddPlayer(PlayerModel model)
        {
            pairs.AddOrUpdate(model.Id, model, (_, __) => model);
        }

        public static PlayerModel GetPlayer(string id)
        {
            PlayerModel v;
            pairs.TryGetValue(id, out v);
            return v;
        }

        public static void RemovePlayer(string id)
        {
            PlayerModel v;
            pairs.TryRemove(id, out v);
        }
    }
}