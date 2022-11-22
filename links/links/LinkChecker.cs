using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace links
{
    internal class LinkChecker
    {
        private readonly string _domain;
        private readonly HttpClient _client;

        public LinkChecker(string d)
        {
            var uri = new Uri(d);
            if (!IsUrl(uri.OriginalString))
            {
                Console.WriteLine(d, " not valid link. Ex https://google.com");
                return;
            }

            _domain = d;
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.All };
            _client = new HttpClient(handler) { DefaultRequestVersion = HttpVersion.Version20 };
            _client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
            _client.BaseAddress = uri;
        }

        public readonly Dictionary<string, int> ValidLinks = new();

        public readonly Dictionary<string, int> InvalidLinks = new();

        private static int GetStatusCode(string link)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(link);
                request.Method = WebRequestMethods.Http.Head;
                request.AllowAutoRedirect = false;
                request.Accept = @"*/*";
                using var response = (HttpWebResponse)request.GetResponse();
                return (int)response.StatusCode;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw;
                return (int)((HttpWebResponse)ex.Response).StatusCode;
            }
        }

        private async Task<IEnumerable<string>> GetLinksFromPage(string link, Uri domain)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, link);
                using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var htmlSnippet = new HtmlDocument();
                if (!(!response.IsSuccessStatusCode || response.Content == null ||
                      response.Content.Headers.ContentType.MediaType != "text/html"))
                {
                    htmlSnippet.LoadHtml(await response.Content.ReadAsStringAsync());
                }

                var links = new List<string>();

                foreach (var a in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var href = a.Attributes["href"];
                    var validLink = GetUrlPath(href.Value, domain);
                    if (!string.IsNullOrEmpty(validLink))
                    {
                        links.Add(validLink);
                    }
                }

                return links;
            }
            catch (Exception)
            {
                Console.WriteLine($"can't download html from {link}");
                return new HashSet<string>();
            }
        }

        private static bool IsValidLink(int status)
        {
            return ((status / 200 == 1) && (status % 200 < 100));
            // return !((status / 400 == 1) && (status % 400 < 100));
        }

        public async Task CheckAllDomainLinks()
        {
            try
            {
                var uri = new Uri(_domain);
                if (!IsUrl(uri.OriginalString))
                {
                    Console.WriteLine(_domain, " not valid uri. Ex https://google.com");
                    return;
                }

                var unverifiedUrls = new HashSet<string> { _domain };
                var verifiedUrls = new HashSet<string>();
                while (unverifiedUrls.Count != 0)
                {
                    var s = unverifiedUrls.ElementAt(unverifiedUrls.Count - 1);
                    unverifiedUrls.Remove(s);
                    verifiedUrls.Add(s);
                    Console.Write($"{s}\t");
                    var status = GetStatusCode(s);
                    Console.WriteLine(status);
                    if (IsValidLink(status))
                    {
                        ValidLinks.Add(s, status);
                        var links = await GetLinksFromPage(s, uri);
                        foreach (var link in links.Where(link => !verifiedUrls.Contains(link)))
                        {
                            unverifiedUrls.Add(link);
                        }
                    }
                    else
                    {
                        InvalidLinks.Add(s, status);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine(_domain, " not valid uri. Ex https://google.com");
            }
        }

        private static bool IsUrl(string s)
        {
            const string pattern = @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?(/)?$";
            // var pattern =
            // @"((http(s)?://)?([\w-]+\.)+[\w-]+[.com]+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
            return Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase);
        }

        private static string GetUrlPath(string s, Uri domain)
        {
            try
            {
                var u = new Uri(s);
                var path = u.Scheme + "://" + u.Host + u.PathAndQuery;
                return (IsUrl(path) && domain.Host == u.Host) ? path : "";
            }
            catch (Exception)
            {
                try
                {
                    var u = new Uri(domain, s);
                    var path = u.Scheme + "://" + u.Host + u.PathAndQuery;
                    return (IsUrl(path) && domain.Host == u.Host) ? path : "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
    }
}