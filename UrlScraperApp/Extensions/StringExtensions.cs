using HtmlAgilityPack;

namespace UrlScraperApp.Extensions
{
    public static class StringExtensions
    {
        //Removes '/' from beginning and end of url string
        public static string CleanUrlEndings(this string url)
        {
            var urlString = url;
            var unwantedCharacter = '/';

            urlString = urlString.TrimStart(unwantedCharacter).TrimEnd(unwantedCharacter);

            return urlString;
        }

        //Creates HtmlNode of requested url
        public static HtmlNode GetHtml(this string url)
        {
            HtmlWeb web = new();
            var htmlDoc = web.Load(url);
            return htmlDoc.DocumentNode;
        }
    }
}
