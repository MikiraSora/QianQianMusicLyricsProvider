using LyricsFinder.SourcePrivoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QianQianMusicLyricsProvider
{
    public class SearchResult : SearchSongResultBase
    {
        public string title;
        public string artist;
        public int duration;
        public string id;

        public override string Title => title;

        public override string Artist => artist;

        public override int Duration => duration;

        public override string ID => id;

        public string LyricsDownloadLink { get; set; }
    }
}
