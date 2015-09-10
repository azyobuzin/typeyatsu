using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Livet;

namespace Typeyatsu
{
    class MainWindowViewModel : KeyEventViewModel
    {
        public MainWindowViewModel()
        {
            this.contentViewModel = new TitlePageViewModel(this);
        }

        private ViewModel contentViewModel;
        public ViewModel ContentViewModel
        {
            get
            {
                return this.contentViewModel;
            }
            set
            {
                this.contentViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            (this.contentViewModel as KeyEventViewModel)?.OnKeyDown(e);
            base.OnKeyDown(e);
        }
    }
}
