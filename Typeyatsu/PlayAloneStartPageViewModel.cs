using System.Text;

namespace Typeyatsu
{
    class PlayAloneStartPageViewModel : KeyEventViewModel
    {
        public PlayAloneStartPageViewModel(MainWindowViewModel parent)
        {
            this.Parent = parent;
        }

        public MainWindowViewModel Parent { get; }

        private string testInput = "";
        public string TestInput
        {
            get
            {
                return this.testInput;
            }
            private set
            {
                this.testInput = value;
                this.RaisePropertyChanged();
            }
        }

        public void AddTestInput(char c)
        {
            var s = new StringBuilder(this.TestInput);
            s.Append(c);
            if (s.Length > 30)
                s.Remove(0, 1);
            this.TestInput = s.ToString();
        }

        public void GoNext()
        {
            this.Parent.ContentViewModel = new PlayAlonePageViewModel(this.Parent);
        }

        public void GoBack()
        {
            this.Parent.ContentViewModel = new TitlePageViewModel(this.Parent);
        }
    }
}
