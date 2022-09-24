using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner.DataStudio.Serialize
{
    [Serializable]
    public class SongData
    {
        public int id;
        public string name;
        public string artist;
        public int level;

        public SongData(int id, string name, string artist, int level)
        {
            this.id = id;
            this.name = name;
            this.artist = artist;
            this.level = level;
        }
    }
}
