using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Typeyatsu.Core;

namespace Typeyatsu.Server
{
    public class GameHub : Hub<IGameHubClient>
    {
        public override Task OnConnected()
        {
            var rivalId = PlayerQueue.MatchOrEnqueue(this.Context.ConnectionId);
            var m = new PlayerModel(this.Context.ConnectionId) { Rival = rivalId };
            PairManager.AddPlayer(m);

            if (rivalId != null)
            {
                PairManager.GetPlayer(rivalId).Rival = this.Context.ConnectionId;

                this.Clients.Caller.OnRivalFound();
                this.Clients.Client(rivalId).OnRivalFound();
            }

            return Task.FromResult(true);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (PlayerQueue.WaitingId == this.Context.ConnectionId)
                PlayerQueue.WaitingId = null;

            var m = PairManager.GetPlayer(this.Context.ConnectionId);
            if (m.Rival != null)
                this.Clients.Client(m.Rival).OnRivalDisconnected();

            PairManager.RemovePlayer(this.Context.ConnectionId);

            return Task.FromResult(true);
        }
    }
}