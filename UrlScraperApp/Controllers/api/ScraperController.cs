using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using UrlScraperApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UrlScraperApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperController : ControllerBase
    {
        ////GET: api/<ScraperController>
        [HttpGet]
        public IActionResult Get()
        {
            var url = "";

            if (string.IsNullOrEmpty(url)) return BadRequest();

            var response = LoadUrl(url);

            if (response != null)
            {
                return new JsonResult(response);
            }

            return BadRequest();
        }

        /// <summary>
        /// Gets images, word count and top words from requested url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>UrlContent</returns>
        public static UrlContent LoadUrl(string url)
        {
            var topWordNum = 10;
            var document = GetHtml(url);
            var totalWordCount = GetWords(document).Count;
            var topWords = totalWordCount >= topWordNum
                           ? GetWords(document).OrderByDescending(f => f.WordCount).Take(topWordNum).ToList()
                           : GetWords(document).OrderByDescending(f => f.WordCount).ToList();

            var result = new UrlContent
            {
                Images = GetImages(document, url),
                TotalWordCount = totalWordCount,
                TopWords = topWords
            };

            return result;
        }

        /// <summary>
        /// Gets images and alt text in html document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="url"></param>
        /// <returns>List<Image></returns>
        public static List<Image> GetImages(HtmlNode document, string url)
        {
            List<Image> images = new List<Image>();
            var imageNodes = document.SelectNodes("//img");

            if (imageNodes == null) { return images; };

            foreach (var imageNode in imageNodes)
            {
                if (!(imageNode?.Attributes[@"src"]?.Value == null))
                {
                    var imageUrl = imageNode.Attributes[@"src"]?.Value?.ToString();
                    Image image = new Image
                    {
                        Url = !imageUrl.StartsWith("http") ? CleanUrlEndings(url) + "/" + CleanUrlEndings(imageUrl) : imageUrl,
                        AltText = imageNode.Attributes[@"alt"]?.Value == null ? "" : imageNode.Attributes[@"alt"].Value.ToString()
                    };

                    images.Add(image);
                };
            }
            return images;
        }

        /// <summary>
        /// Gets list of all words in html document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns>List<Word></returns>
        public static List<Word> GetWords(HtmlNode document)
        {
            List<Word> words = new List<Word>();
            var innerText = document.InnerText.Split();

            if ((innerText == null)) { return words; };

            foreach (var text in innerText)
            {
                Word word = new();

                if (words.Any(f => f.Text == text))
                {
                    words.Where(f => f.Text == text).ToList().ForEach(f => f.WordCount++);
                }
                else if (!string.IsNullOrWhiteSpace(text) && Regex.IsMatch(text, @"^[a-zA-Z]+$"))
                {
                    word.Text = text;
                    word.WordCount = 1;
                    words.Add(word);
                }
            }

            return words;
        }

        //Creates HtmlNode of requested url
        public static HtmlNode GetHtml(string url)
        {
            HtmlWeb web = new();
            var htmlDoc = web.Load(url);
            return htmlDoc.DocumentNode;
        }

        //Removes '/' from beginning and end of url string
        public static string CleanUrlEndings(string url)
        {
            var urlString = url;
            var unwantedCharacter = '/';

            urlString = urlString.TrimStart(unwantedCharacter).TrimEnd(unwantedCharacter);

            return urlString;
        }
    }
}
