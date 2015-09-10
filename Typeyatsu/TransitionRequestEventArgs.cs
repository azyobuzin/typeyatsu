using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Typeyatsu
{
    class TransitionRequestEventArgs : EventArgs
    {
        public TransitionRequestEventArgs(UserControl newContent)
        {
            this.NewContent = newContent;
        }

        public UserControl NewContent { get; }
    }
}
