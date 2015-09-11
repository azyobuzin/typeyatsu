using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Typeyatsu.Core;

namespace Typeyatsu.Server
{
    public class GameHub : Hub<IGameHubClient>, IGameHub
    {
        public override Task OnConnected()
        {
            var rivalId = PlayerQueue.MatchOrEnqueue(this.Context.ConnectionId);
            var m = new PlayerModel(this.Context.ConnectionId) { Rival = rivalId };
            PairManager.AddPlayer(m);

            if (rivalId != null)
            {
                PairManager.GetPlayer(rivalId).Rival = this.Context.ConnectionId;

                var words = Keywords.GetRandomWords(10);
                this.Clients.Clients(new[] { this.Context.ConnectionId, rivalId }).OnRivalFound(words);
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

        public void NotifyState(GameState state)
        {
            var m = PairManager.GetPlayer(this.Context.ConnectionId);
            m.LastState = state;
            this.Clients.Client(m.Rival).OnRivalStateChanged(state);
        }

        public GameState GetLastRivalState()
        {
            return PairManager.GetPlayer(PairManager.GetPlayer(this.Context.ConnectionId).Rival).LastState;
        }

        public void NotifyGameOver(GameResult result)
        {
            this.Clients.Client(PairManager.GetPlayer(this.Context.ConnectionId).Rival)
                .OnRivalGameOver(result);
        }
    }
}