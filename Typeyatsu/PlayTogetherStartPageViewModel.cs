using System;
using System.Text;
using System.Windows.Threading;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;
using Microsoft.AspNet.SignalR.Client;
using Typeyatsu.Core;

namespace Typeyatsu
{
    class PlayTogetherStartPageViewModel : KeyEventViewModel
    {
        public PlayTogetherStartPageViewModel(MainWindowViewModel parent)
        {
            this.Parent = parent;
            this.CompositeDisposable.Add(this.hubListeners);
        }

#if DEBUG_LOCAL
        private const string ServerAddress = "http://localhost:50934/";
#else
        private const string ServerAddress = "http://typeyatsu.azurewebsites.net/";
#endif

        private HubConnection connection;
        private IHubProxy hub;

        private readonly LivetCompositeDisposable hubListeners = new LivetCompositeDisposable();

        public MainWindowViewModel Parent { get; }

        private Keyword[] words;

        private bool isRivalFound;
        public bool IsRivalFound
        {
            get
            {
                return this.isRivalFound;
            }
            private set
            {
                this.isRivalFound = value;
                this.RaisePropertyChanged();
            }
        }

        private string testInput = "";
        public string TestInput
        {
            get
            {
                return this.testInput;
            }
            private set
            {
                this.testInput = value;
                this.RaisePropertyChanged();
            }
        }

        public void AddTestInput(char c)
        {
            var s = new StringBuilder(this.TestInput);
            s.Append(c);
            if (s.Length > 30)
                s.Remove(0, 1);
            this.TestInput = s.ToString();
        }

        private DispatcherTimer remainingTimeTimer;

        private int remainingTime = 5;
        public int RemainingTime
        {
            get
            {
                return this.remainingTime;
            }
            private set
            {
                this.remainingTime = value;
                this.RaisePropertyChanged();
            }
        }

        private void OnDisconnected()
        {
            this.StopAll();
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                this.Messenger.Raise(new InteractionMessage("MsgDisconnected"));
                this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
            });
        }

        public void Initialize()
        {
            this.connection = new HubConnection(ServerAddress);
            this.hub = this.connection.CreateHubProxy("GameHub");

            this.hubListeners.Add(new EventListener<Action>(
                x => this.connection.Closed += x,
                x => this.connection.Closed -= x,
                this.OnDisconnected));

            this.hubListeners.Add(this.hub.On(nameof(IGameHubClient.OnRivalDisconnected), this.OnDisconnected));

            this.hubListeners.Add(this.hub.On(nameof(IGameHubClient.OnRivalFound), new Action<Keyword[]>(this.OnRivalFound)));

            this.connection.Start();
        }

        private void StopAll()
        {
            this.remainingTimeTimer?.Stop();
            this.hubListeners.Dispose();
            this.connection?.Dispose();
        }

        public void GoBack()
        {
            this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
            this.StopAll();
        }

        private void OnRivalFound(Keyword[] words)
        {
            this.words = words;
            this.IsRivalFound = true;
            this.remainingTimeTimer = new DispatcherTimer(DispatcherPriority.Normal, DispatcherHelper.UIDispatcher);
            this.remainingTimeTimer.Interval = new TimeSpan(TimeSpan.TicksPerSecond * 1);
            this.remainingTimeTimer.Tick += (sender, e) =>
            {
                if (--this.RemainingTime <= 0)
                    this.GoNext();
            };
            this.remainingTimeTimer.Start();
        }

        private void GoNext()
        {
            this.remainingTimeTimer?.Stop();
            this.hubListeners.Dispose();

            this.Parent.ContentViewModel = new PlayTogetherPageViewModel(this.Parent, this.connection, this.hub, this.words);
        }
    }
}
