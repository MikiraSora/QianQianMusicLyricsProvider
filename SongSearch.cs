using HtmlAgilityPack;
using LyricsFinder;
using LyricsFinder.SourcePrivoder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QianQianMusicLyricsProvider
{
    public class SongSearch : SongSearchBase<SearchResult>
    {
        const string SEARCH_API_URL = @"http://music.taihe.com/search?key={0}";
        const string SONG_INFO_API_URL = @"http://music.taihe.com/data/tingapi/v1/restserver/ting?method=baidu.ting.song.baseInfo&songid={0}&from=web";

        public override List<SearchResult> Search(params string[] param_arr)
        {
            string title = param_arr[0], artist = param_arr[1];

            var url = string.Format(SEARCH_API_URL, $"{artist} {title}");

            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Timeout=Setting.SearchAndDownloadTimeout;

            using (var stream=request.GetResponse().GetResponseStream())
            {
                HtmlDocument document = new HtmlDocument();
                document.Load(stream);

                var infos =
                    document.
                    DocumentNode.
                    SelectNodes("//li[@data-songitem]")?.ToList()??new List<HtmlNode>();

                var task=Task.WhenAll(infos.Select(l => ParseContent(l)));

                task.Wait();
                return task.Result.ToList();
            }
        }

        readonly static Regex reg=new Regex(@"\d+");

        private async Task<SearchResult> ParseContent(HtmlNode node)
        {
            var json_content=node.SelectNodes("div/span/a").First().Attributes["data-songdata"].Value;
            JObject result;

            try
            {
                result = JsonConvert.DeserializeObject(json_content) as JObject;

                if (result==null)
                    return null;
            }
            catch
            {
                return null;
            }

            var id = result["id"].ToString();
            HttpWebRequest request = WebRequest.CreateHttp(string.Format(SONG_INFO_API_URL, id));
            request.Timeout=Setting.SearchAndDownloadTimeout;

            var response= await request.GetResponseAsync();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var content = await reader.ReadToEndAsync();
                var r = (JsonConvert.DeserializeObject(content) as JObject)?["content"];

                return r==null ? null : new SearchResult()
                {
                    title=r["title"].ToString(),
                    artist=r["author"].ToString(),
                    id=id,
                    duration=r["file_duration"].ToObject<int>()*1000, // convert to ms
                    LyricsDownloadLink=r["lrclink"].ToString()
                };
            }
        }
    }
}
