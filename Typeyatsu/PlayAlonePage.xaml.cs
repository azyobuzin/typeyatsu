using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Typeyatsu
{
    /// <summary>
    /// PlayAlonePage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayAlonePage : UserControl
    {
        public PlayAlonePage()
        {
            InitializeComponent();
        }

        private PlayAlonePageViewModel ViewModel => this.DataContext as PlayAlonePageViewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.WindowKeyDown += (_, f) =>
            {
                if (f.Key == Key.Escape)
                {
                    this.ViewModel.GoBack();
                    return;
                }

                char c;
                if (KeyboardHelper.TryKeyToChar(f.Key, out c))
                    this.ViewModel.Input(c);
            };

            this.ViewModel.Start();
        }
    }
}
