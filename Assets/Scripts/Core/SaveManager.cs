using Runner.DataStudio.Serialize;
using Runner.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Runner.Core
{
    /// <summary>
    /// 存档管理器: SaveManager
    /// Author: 单元琛 2022-8-25
    /// </summary>
    public class SaveManager : Singleton<SaveManager>
    {
        private PlayerData playerData;

        protected override void OnAwake()
        {
            base.OnAwake();
            try
            {
                playerData = SaveHandler.GetSave();
            }
            catch (FileNotFoundException)
            {
                playerData = SaveHandler.InitSave();
            }
        }

        public void SaveData()
        {
            SaveHandler.SavePrefs(playerData);
        }

        public void ResetData()
        {
            playerData = SaveHandler.InitSave();
        }

        public PlayerData GetPlayerData() => playerData;

        public void SetVolume(float music, float se)
        {
            playerData.musicVolume = music;
            playerData.seVolume = se;
            SaveData();
        }

        public void SetShowMiddle(bool b)
        {
            playerData.showMiddle = b;
            SaveData();
        }

        public Dictionary<int, LevelData> GetUserLevelDatas()
        {
            var songlist = TableManager.Instance.GetSongList();
            foreach (var item in songlist)
            {
                if (!playerData.levelDatas.ContainsKey(item.Musicid))
                {
                    playerData.levelDatas.Add(item.Musicid, new LevelData(item.Musicid));
                }
            }
            SaveData();
            return playerData.levelDatas;
        }

        public void SaveLevelData(int id, int collection, int score)
        {
            var currentData = GetUserLevelDatas()[id];
            LevelData levelData = new(id)
            {
                collection = collection > currentData.collection ? collection : currentData.collection,
                score = score > currentData.score ? score : currentData.score,
                isPassed = true,
            };
            playerData.levelDatas[id] = levelData;
            SaveData();
        }

        public float GetSEVolume() => (float)playerData.seVolume;
    }

}