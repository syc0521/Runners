using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner.DataStudio.Serialize
{
    [Serializable]
    public class PlayerData
    {
        public double musicVolume, seVolume;
        public bool showMiddle = false;
        public Dictionary<int, LevelData> levelDatas;
        public PlayerData()
        {
            musicVolume = 0.9f;
            seVolume = 0.9f;
            showMiddle = false;
            levelDatas = new();
        }
    }

    [Serializable]
    public struct LevelData
    {
        public int id;
        public bool isPassed;
        public int collection;
        public int score;

        public LevelData(int id)
        {
            this.id = id;
            isPassed = false;
            collection = 0;
            score = 0;
        }
    }
}
