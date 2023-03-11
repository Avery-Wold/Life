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