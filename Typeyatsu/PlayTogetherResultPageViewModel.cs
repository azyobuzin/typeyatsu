using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet.Commands;
using Typeyatsu.Core;

namespace Typeyatsu
{
    class PlayTogetherResultPageViewModel : KeyEventViewModel
    {
        public PlayTogetherResultPageViewModel(MainWindowViewModel parent, TimeSpan time, int typeCount, int mistypeCount, GameResult rivalResult)
        {
            this.Parent = parent;
            this.time = time;
            this.typePerSec = typeCount / time.TotalSeconds;
            this.MistypeCount = mistypeCount;

            if (rivalResult == null)
            {
                this.IsRivalDisconnected = true;
            }
            else
            {
                this.rivalTime = rivalResult.Time;
                this.rivalTypePerSec = rivalResult.TypeCount / rivalResult.Time.TotalSeconds;
                this.RivalMistypeCount = rivalResult.MistypeCount;
            }
        }

        public MainWindowViewModel Parent { get; }

        private readonly TimeSpan time;
        public string Time => this.time.TotalSeconds.ToString("F3");

        private readonly double typePerSec;
        public string TypePerSec => this.typePerSec.ToString("F3");

        public int MistypeCount { get; }

        public bool IsRivalDisconnected { get; }

        private readonly TimeSpan rivalTime;
        public string RivalTime => this.rivalTime.TotalSeconds.ToString("F3");

        private readonly double rivalTypePerSec;
        public string RivalTypePerSec => this.rivalTypePerSec.ToString("F3");

        public int RivalMistypeCount { get; }

        public void GoBack()
        {
            this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
        }

        private ViewModelCommand goBackCommand;
        public ViewModelCommand GoBackCommand =>
            this.goBackCommand ?? (this.goBackCommand = new ViewModelCommand(this.GoBack));
    }
}
