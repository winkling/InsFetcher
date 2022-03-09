using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace InsFetcher
{
    class Program
    {
        static Dictionary<string, string> urlList = new Dictionary<string, string>();

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string sourceURL = args[0];
            //string sourceURL = @"https://www.instagram.com/p/CMENJUll6pp/";

            WebClient wc = new WebClient();
            var sourceHtml = await wc.DownloadStringTaskAsync(sourceURL);
            //File.WriteAllText("sample.html", sourceHtml);

            SearchForValidURL(sourceHtml);

            if (urlList.Count > 0)
            {
                foreach (var kvp in urlList)
                {
                    Console.Write("Downloading {0} ...", kvp.Key);
                    await wc.DownloadFileTaskAsync(kvp.Value, kvp.Key);
                    Console.WriteLine(" done.");
                }
            }
            else
            {
                Console.WriteLine("No valid url found.");
            }
        }

        static void SearchForValidURL(string source)
        {
            if (string.IsNullOrEmpty(source))
                return;

            var startIndex = source.IndexOf("https:");
            if (startIndex < 0)
                return;
            var endIndex = source.IndexOf("\"", startIndex);
            var str = source.Substring(startIndex, endIndex - startIndex);

            if (!string.IsNullOrEmpty(str))
            {
                if (str.Contains(".jpg") && str.Contains("p1080"))
                {
                    str = str.Replace("\\u0026", "&");
                    var filename = GetJpgFileName(str);
                    urlList[filename] = str;
                }
                else if (str.Contains(".mp4"))
                {
                    str = str.Replace("\\u0026", "&");
                    var filename = GetMp4FileName(str);
                    urlList[filename] = str;
                }
            }

            SearchForValidURL(source.Substring(endIndex));
        }

        static string GetJpgFileName(string url)
        {
            var startIndex = url.IndexOf(".jpg");
            url = url.Substring(0, startIndex + 4);
            startIndex = url.LastIndexOf("/");

            return url.Substring(startIndex + 1);

        }

        static string GetMp4FileName(string url)
        {
            var startIndex = url.IndexOf(".mp4");
            url = url.Substring(0, startIndex + 4);
            startIndex = url.LastIndexOf("/");

            return url.Substring(startIndex + 1);
        }
    }
}
