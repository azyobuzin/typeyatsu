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
    /// PlayAloneStartPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayAloneStartPage : UserControl
    {
        public PlayAloneStartPage()
        {
            InitializeComponent();
        }

        private PlayAloneStartPageViewModel ViewModel => this.DataContext as PlayAloneStartPageViewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.WindowKeyDown += (_, f) =>
            {
                switch (f.Key)
                {
                    case Key.Enter:
                    case Key.Space:
                        this.ViewModel.GoNext();
                        break;
                    case Key.Escape:
                        this.ViewModel.GoBack();
                        break;
                    default:
                        char c;
                        if (KeyboardHelper.TryKeyToChar(f.Key, out c))
                            this.ViewModel.AddTestInput(c);
                        break;
                }
            };
        }
    }
}
