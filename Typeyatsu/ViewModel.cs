using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Livet.Commands;

namespace Typeyatsu
{
    public class ViewModel
    {
        public static ViewModel Instance { get; } = new ViewModel();

        private void PlayAlone()
        {
            
        }

        private ViewModelCommand playAloneCommand;
        public ViewModelCommand PlayAloneCommand =>
            this.playAloneCommand ?? (this.playAloneCommand = new ViewModelCommand(this.PlayAlone));
    }
}
