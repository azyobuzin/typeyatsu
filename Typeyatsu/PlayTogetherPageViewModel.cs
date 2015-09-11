using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Typeyatsu.Core;

namespace Typeyatsu
{
    class PlayTogetherPageViewModel : KeyEventViewModel
    {
        public PlayTogetherPageViewModel(MainWindowViewModel parent, HubConnection connection, IHubProxy hub, Keyword[] words)
        {
            this.Parent = parent;
            this.connection = connection;
            this.hub = hub;
            this.words = words;
            this.CompositeDisposable.Add(this.hubListeners);
            this.CompositeDisposable.Add(connection);
        }

        public MainWindowViewModel Parent { get; }
        private readonly HubConnection connection;
        private readonly IHubProxy hub;
        private readonly Keyword[] words;

        private int index = -1;

        public int DisplayIndex => this.index + 1;

        public int Count => this.words.Length;

        private int typeCount;
        private int mistypeCount;

        private bool isGameOver;

        private readonly Stopwatch time = new Stopwatch();
        private DispatcherTimer propertyChangedTimer;

        public string TimeString => this.time.Elapsed.TotalSeconds.ToString("F1");

        public string Furigana => this.words?[this.index].Furigana;
        public string Word => this.words?[this.index].Word;

        private List<string> availableRomajis;

        private string typedRomaji;
        public string TypedRomaji
        {
            get
            {
                return this.typedRomaji;
            }
            private set
            {
                this.typedRomaji = value;
                this.RaisePropertyChanged();
            }
        }

        private string remainingRomaji;
        public string RemainingRomaji
        {
            get
            {
                return this.remainingRomaji;
            }
            private set
            {
                this.remainingRomaji = value;
                this.RaisePropertyChanged();
            }
        }

        private readonly LivetCompositeDisposable hubListeners = new LivetCompositeDisposable();

        private bool isRivalDisconnected;

        private GameState rivalState;
        public string RivalFurigana => this.rivalState == null ? null : this.words[this.rivalState.Index].Furigana;
        public string RivalWord => this.rivalState == null ? null : this.words[this.rivalState.Index].Word;
        public string RivalTypedRomaji => this.rivalState?.TypedRomaji;
        public string RivalRemainingRomaji => this.rivalState?.RemainingRomaji;

        private GameResult rivalResult;

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public async void Start()
        {
            this.hubListeners.Add(new EventListener<Action>(
               x => this.connection.Closed += x,
               x => this.connection.Closed -= x,
               this.OnDisconnected));

            this.hubListeners.Add(this.hub.On(nameof(IGameHubClient.OnRivalDisconnected), this.OnDisconnected));

            this.hubListeners.Add(this.hub.On(nameof(IGameHubClient.OnRivalGameOver), new Action<GameResult>(this.OnRivalGameOver)));

            this.propertyChangedTimer = new DispatcherTimer(DispatcherPriority.Normal, DispatcherHelper.UIDispatcher);
            this.propertyChangedTimer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * 50);
            this.propertyChangedTimer.Tick += (sender, e) => this.RaisePropertyChanged(nameof(this.TimeString));
            this.propertyChangedTimer.Start();

            this.Next();

            this.time.Start();

            this.hub.On(nameof(IGameHubClient.OnRivalStateChanged), new Action<GameState>(this.UpdateRivalState));
            this.UpdateRivalState(await this.hub.Invoke<GameState>(nameof(IGameHub.GetLastRivalState)));
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        private void UpdateRivalState(GameState state)
        {
            if (state == null) return;
            this.rivalState = state;
            this.RaisePropertyChanged(nameof(this.RivalFurigana));
            this.RaisePropertyChanged(nameof(this.RivalWord));
            this.RaisePropertyChanged(nameof(this.RivalTypedRomaji));
            this.RaisePropertyChanged(nameof(this.RivalRemainingRomaji));
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        private void Next()
        {
            this.index++;

            if (this.index >= this.words.Length)
            {
                this.index--;
                this.GameOver();
                return;
            }

            this.availableRomajis = RomajiGenerator.CreateRomaji(this.words[this.index].Furigana)
                .OrderBy(x => x.Length).ToList();

            this.RaisePropertyChanged(nameof(this.DisplayIndex));
            this.RaisePropertyChanged(nameof(this.Furigana));
            this.RaisePropertyChanged(nameof(this.Word));

            this.TypedRomaji = "";
            this.RemainingRomaji = this.availableRomajis[0];

            this.NotifyState();
        }

        private Task NotifyState()
        {
            if (!this.isRivalDisconnected)
            {
                return this.hub.Invoke(nameof(IGameHub.NotifyState), new GameState()
                {
                    Index = this.index,
                    TypedRomaji = this.TypedRomaji,
                    RemainingRomaji = this.RemainingRomaji
                });
            }
            return Task.FromResult(true);
        }

        private async void GameOver()
        {
            this.time.Stop();
            this.isGameOver = true;
            this.propertyChangedTimer?.Stop();

            if (!this.isRivalDisconnected)
            {
                await Task.WhenAll(
                    this.NotifyState(),
                    this.hub.Invoke(nameof(IGameHub.NotifyGameOver), new GameResult()
                    {
                        Time = this.time.Elapsed,
                        TypeCount = this.typeCount,
                        MistypeCount = this.mistypeCount
                    }));
            }

            if (this.isRivalDisconnected || this.rivalResult != null)
                this.GoNext();
        }

        private void StopAll()
        {
            this.time.Stop();
            this.propertyChangedTimer?.Stop();
            this.connection.Dispose();
        }

        private void OnDisconnected()
        {
            this.isRivalDisconnected = true;

            if (this.isGameOver) this.GoNext();
        }

        public void Input(char c)
        {
            if (!this.time.IsRunning) return;

            var newInput = this.typedRomaji + c;
            var remaining = new List<string>();
            foreach (var s in this.availableRomajis)
            {
                if (s.Equals(newInput, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.typeCount++;
                    this.TypedRomaji = s;
                    this.RemainingRomaji = "";
                    this.Next();
                    return;
                }

                if (s.StartsWith(newInput, StringComparison.InvariantCultureIgnoreCase))
                    remaining.Add(s);
            }

            if (remaining.Count == 0)
            {
                this.mistypeCount++;
                return;
            }

            this.typeCount++;

            this.TypedRomaji = newInput;
            this.RemainingRomaji = remaining[0].Substring(newInput.Length);

            this.NotifyState();
        }

        private void OnRivalGameOver(GameResult result)
        {
            this.rivalResult = result;

            if (this.isGameOver) this.GoNext();
        }

        private void GoNext()
        {
            this.StopAll();
            this.Parent.ContentViewModel = new PlayTogetherResultPageViewModel(
                this.Parent, this.time.Elapsed, this.typeCount, this.mistypeCount, this.rivalResult);
        }

        public void GoBack()
        {
            this.StopAll();
            this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
        }
    }
}
