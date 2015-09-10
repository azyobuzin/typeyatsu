using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Livet.Commands;
using Typeyatsu.Core;

namespace Typeyatsu
{
    class TitlePageViewModel : ViewModel
    {
        public TitlePageViewModel(MainWindowViewModel parent)
        {
            this.Parent = parent;
        }

        public MainWindowViewModel Parent { get; }

        private void PlayAlone()
        {
            this.Parent.ContentViewModel = new PlayAloneStartPageViewModel(this.Parent);
        }

        private ViewModelCommand playAloneCommand;
        public ViewModelCommand PlayAloneCommand =>
            this.playAloneCommand ?? (this.playAloneCommand = new ViewModelCommand(this.PlayAlone));

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            private set
            {
                this.isLoading = value;
                this.RaisePropertyChanged();
            }
        }

        private async void Initialize()
        {
            if (Keywords.Hatena == null)
            {
                this.IsLoading = true;
                await Task.Run(new Action(Keywords.Load));
                this.IsLoading = false;
            }
        }

        private ViewModelCommand initializeCommand;
        public ViewModelCommand InitializeCommand =>
            this.initializeCommand ?? (this.initializeCommand = new ViewModelCommand(this.Initialize));
    }
}
