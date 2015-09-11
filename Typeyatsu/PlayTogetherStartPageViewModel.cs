using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Livet.Commands;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Threading;
using Livet.EventListeners;
using Livet.Messaging;

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

        private readonly LivetCompositeDisposable hubListeners = new LivetCompositeDisposable();

        public MainWindowViewModel Parent { get; }

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
            System.Diagnostics.Debugger.Break();
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                this.Messenger.Raise(new InteractionMessage("MsgDisconnected"));
                this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
            });
        }

        public void Initialize()
        {
            this.connection = new HubConnection(ServerAddress);
            var hub = this.connection.CreateHubProxy("GameHub");
            
            this.hubListeners.Add(new EventListener<Action>(
                x => this.connection.Closed += x,
                x => this.connection.Closed -= x,
                this.OnDisconnected));

            this.connection.StateChanged += s => System.Diagnostics.Trace.WriteLine($"State {s.OldState} -> {s.NewState}");

            this.hubListeners.Add(hub.On("OnRivalDisconnected", this.OnDisconnected));

            this.hubListeners.Add(hub.On("OnRivalFound", this.OnRivalFound));

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

        private void OnRivalFound()
        {
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
            this.StopAll();
        }
    }
}
