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
    /// PlayTogetherStartPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayTogetherStartPage : UserControl
    {
        public PlayTogetherStartPage()
        {
            InitializeComponent();
        }

        private PlayTogetherStartPageViewModel ViewModel => this.DataContext as PlayTogetherStartPageViewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.WindowKeyDown += (_, f) =>
            {
                switch (f.Key)
                {
                    case Key.Escape:
                        this.ViewModel.GoBack();
                        e.Handled = true;
                        break;
                    default:
                        char c;
                        if (KeyboardHelper.TryKeyToChar(f.Key, out c))
                        {
                            this.ViewModel.AddTestInput(c);
                            e.Handled = true;
                        }
                        break;
                }
            };

            this.ViewModel.Initialize();
        }
    }
}
