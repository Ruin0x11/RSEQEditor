using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSEQEditor
{
    public static class WiiMusic
    {
        public class WiiMusicSong
        {
            public string DisplayName;
            public string Path;

            public WiiMusicSong(string displayName, string path)
            {
                DisplayName = displayName;
                path = Path;
            }
        }
        public static readonly List<WiiMusicSong> SONGS = new List<WiiMusicSong>
        {

        };
    }
}
