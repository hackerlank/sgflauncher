using System.Collections.Generic;
using System.Windows.Forms;

namespace Aion_Launcher
{
    class NewsParser
    {
        public List<NewsLink> newsLinks     = new List<NewsLink>();
        bool newsUpdateRequired             = true;

        public void parseNews(WebBrowser web)
        {
            if (newsUpdateRequired && Settings.newsEnabled)
            {
                try
                {
                    foreach (HtmlElement link in web.Document.Links)
                    {
                        string href = link.GetAttribute("href");
                        string text = link.InnerText;
                        if (href.Contains("news-") && !text.Contains("Читать дальше"))
                            newsLinks.Add(new NewsLink(link.InnerText, href));
                        newsUpdateRequired = false;
                    }

                    updateNewsInfo();
                }
                catch { }
            }

        }

        void updateNewsInfo()
        {
            Properties.Settings.Default.news1link = newsLinks[0].Link;
            Properties.Settings.Default.news2link = newsLinks[1].Link;
            Properties.Settings.Default.news3link = newsLinks[2].Link;
            Properties.Settings.Default.news4link = newsLinks[3].Link;
            Properties.Settings.Default.news5link = newsLinks[4].Link;

            Properties.Settings.Default.news1text = newsLinks[0].Text;
            Properties.Settings.Default.news2text = newsLinks[1].Text;
            Properties.Settings.Default.news3text = newsLinks[2].Text;
            Properties.Settings.Default.news4text = newsLinks[3].Text;
            Properties.Settings.Default.news5text = newsLinks[4].Text;

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

    }


}
