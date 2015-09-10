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
    /// PlayAloneResultPage.xaml の相互作用ロジック
    /// </summary>
    public partial class PlayAloneResultPage : UserControl
    {
        public PlayAloneResultPage()
        {
            InitializeComponent();
        }

        private PlayAloneResultPageViewModel ViewModel => this.DataContext as PlayAloneResultPageViewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.WindowKeyDown += (_, f) =>
            {
                switch (f.Key)
                {
                    case Key.Enter:
                    case Key.Space:
                    case Key.Escape:
                        this.ViewModel.GoBack();
                        break;
                }
            };
        }
    }
}
