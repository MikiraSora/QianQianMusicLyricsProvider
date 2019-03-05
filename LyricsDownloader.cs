using LyricsFinder.SourcePrivoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QianQianMusicLyricsProvider
{
    public class LyricsDownloader : LyricDownloaderBase
    {
        public override string DownloadLyric(SearchSongResultBase song, bool request_trans_lyrics = false)
        {
            var info = song as SearchResult;

            if (info==null)
                return null;

            var download_link = info.LyricsDownloadLink;

            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(download_link);

                using (var reader = new StreamReader((request.GetResponse()).GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch 
            {
                return null;
            }
        }
    }
}
