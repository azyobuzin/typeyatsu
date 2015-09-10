using System;
using System.Windows.Input;
using Livet;

namespace Typeyatsu
{
    class KeyEventViewModel : ViewModel
    {
        public virtual void OnKeyDown(KeyEventArgs e)
        {
            this.WindowKeyDown?.Invoke(this, e);
        }

        public event EventHandler<KeyEventArgs> WindowKeyDown;
    }
}
