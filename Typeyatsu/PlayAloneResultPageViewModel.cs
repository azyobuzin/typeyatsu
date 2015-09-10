using System;
using Livet.Commands;

namespace Typeyatsu
{
    class PlayAloneResultPageViewModel : KeyEventViewModel
    {
        public PlayAloneResultPageViewModel(MainWindowViewModel parent, TimeSpan time, int typeCount, int mistypeCount)
        {
            this.Parent = parent;
            this.time = time;
            this.typePerSec = typeCount / time.TotalSeconds;
            this.MistypeCount = mistypeCount;
        }

        public MainWindowViewModel Parent { get; }

        private readonly TimeSpan time;
        public string Time => this.time.TotalSeconds.ToString("F3");

        private readonly double typePerSec;
        public string TypePerSec => this.typePerSec.ToString("F3");

        public int MistypeCount { get; }

        public void GoBack()
        {
            this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
        }

        private ViewModelCommand goBackCommand;
        public ViewModelCommand GoBackCommand =>
            this.goBackCommand ?? (this.goBackCommand = new ViewModelCommand(this.GoBack));
    }
}
