using System;
using System.Diagnostics;

namespace Life.ViewModels
{
	public class HowToPlayViewModel : BaseViewModel
    {
        public Command HyperlinkCommand { get; private set; }

        public HowToPlayViewModel()
        {
            HyperlinkCommand = new Command<string>(async (_link) => await HyperLink());
            Link = "https://en.wikipedia.org/wiki/Conway's_Game_of_Life";
            More = "For more information, see the Wikipedia article:";
            Rule1 = "Any live cell with fewer than two live neighbours dies, as if caused by under-population." + Environment.NewLine;
            Rule2 = "Any live cell with two or three live neighbours lives on to the next generation." + Environment.NewLine;
            Rule3 = "Any live cell with more than three live neighbours dies, as if by overcrowding." + Environment.NewLine;
            Rule4 = "Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.";
        }

        private async Task HyperLink()
        {
            try
            {
                if (!string.IsNullOrEmpty(_link))
                {
                    if (!_link.Trim().StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        _link = "https://" + _link;
                    }
                    if (IsValidUri(_link))
                    {
                        await Browser.OpenAsync(new Uri(_link), BrowserLaunchMode.SystemPreferred);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private string _link;
        public string Link
        {
            get => _link;
            set => _link = value;
        }

        private string _more;
        public string More
        {
            get => _more;
            set => _more = value;
        }

        private string _rule1;
        public string Rule1
        {
            get => _rule1;
            set => _rule1 = value;
        }

        private string _rule2;
        public string Rule2
        {
            get => _rule2;
            set => _rule2 = value;
        }

        private string _rule3;
        public string Rule3
        {
            get => _rule3;
            set => _rule3 = value;
        }

        private string _rule4;
        public string Rule4
        {
            get => _rule4;
            set => _rule4 = value;
        }

        private Boolean IsValidUri(string uri)
        {
            try
            {
                new Uri(uri);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}