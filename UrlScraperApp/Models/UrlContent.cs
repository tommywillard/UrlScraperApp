namespace UrlScraperApp.Models
{
    public class UrlContent
    {
        public List<Image> Images { get; set; }

        public int TotalWordCount { get; set; }

        public List<Word> TopWords { get; set; }
    }
}
