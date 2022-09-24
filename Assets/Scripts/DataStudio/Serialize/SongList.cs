using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner.DataStudio.Serialize
{
    [Serializable]
    public class SongList
    {
        public List<SongData> SongDatas;
        public SongList()
        {
            SongDatas = new();
        }

        public SongList(List<SongData> songDatas)
        {
            SongDatas = songDatas;
        }
    }
}
