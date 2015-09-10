using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Threading;
using Livet;
using Typeyatsu.Core;

namespace Typeyatsu
{
    class PlayAlonePageViewModel : KeyEventViewModel
    {
        public PlayAlonePageViewModel(MainWindowViewModel parent)
        {
            this.Parent = parent;
        }

        public MainWindowViewModel Parent { get; }

        private const int WordsCount = 20;

        private readonly Keyword[] words = new Keyword[WordsCount];

        private int index = -1;

        public int DisplayIndex => this.index + 1;

        public int Count => WordsCount;

        private int typeCount;
        private int mistypeCount;

        private readonly Stopwatch time = new Stopwatch();
        private DispatcherTimer propertyChangedTimer;

        public string TimeString => this.time.Elapsed.TotalSeconds.ToString("F1");

        public string Furigana => this.words[this.index].Furigana;
        public string Word => this.words[this.index].Word;

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

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public void Start()
        {
            var indices = new List<int>(WordsCount);
            while (indices.Count < WordsCount)
            {
                var i = App.Random.Next(Keywords.Hatena.Length);
                if (!indices.Contains(i))
                    indices.Add(i);
            }

            for (var i = 0; i < WordsCount; i++)
                this.words[i] = Keywords.Hatena[indices[i]];

            this.propertyChangedTimer = new DispatcherTimer(DispatcherPriority.Normal, DispatcherHelper.UIDispatcher);
            this.propertyChangedTimer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * 50);
            this.propertyChangedTimer.Tick += (sender, e) => this.RaisePropertyChanged(nameof(this.TimeString));
            this.propertyChangedTimer.Start();

            this.Next();

            this.time.Start();
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
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        private void Next()
        {
            this.index++;

            if (this.index >= WordsCount)
            {
                this.Gameover();
                return;
            }

            this.availableRomajis = RomajiGenerator.CreateRomaji(this.words[this.index].Furigana)
                .OrderBy(x => x.Length).ToList();

            this.RaisePropertyChanged(nameof(this.DisplayIndex));
            this.RaisePropertyChanged(nameof(this.Furigana));
            this.RaisePropertyChanged(nameof(this.Word));

            this.TypedRomaji = "";
            this.RemainingRomaji = this.availableRomajis[0];
        }

        private void StopAll()
        {
            this.time.Stop();
            this.propertyChangedTimer?.Stop();
        }

        private void Gameover()
        {
            this.StopAll();
            this.Parent.ContentViewModel = new PlayAloneResultPageViewModel(
                this.Parent, this.time.Elapsed, this.typeCount, this.mistypeCount);
        }

        public void GoBack()
        {
            this.StopAll();
            this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
        }
    }
}
