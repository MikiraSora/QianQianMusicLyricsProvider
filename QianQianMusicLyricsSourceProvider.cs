using LyricsFinder;
using LyricsFinder.SourcePrivoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QianQianMusicLyricsProvider
{
    [SourceProviderName("qianqian", "MikiraSora")]
    public class QianQianMusicSourceProvider:SourceProviderBase<SearchResult, SongSearch, LyricsDownloader,DefaultLyricsParser>
    {

    }
}
