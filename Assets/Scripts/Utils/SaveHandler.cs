using Runner.DataStudio.Serialize;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Runner.Utils
{
    public static class SaveHandler
    {
        /// <summary>
        /// 保存存档
        /// </summary>
        public static void SavePrefs(PlayerData data)
        {
            var jsonStr = JsonConvert.SerializeObject(data);
            StreamWriter sw = new(Global.savePath);
            sw.Write(jsonStr);
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// 获取存档
        /// </summary>
        public static PlayerData GetSave()
        {
            StreamReader sr = new(Global.savePath);
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(sr.ReadToEnd());
            sr.Close();
            sr.Dispose();
            return data;
        }

        /// <summary>
        /// 初始化存档
        /// </summary>
        public static PlayerData InitSave()
        {
            PlayerData data = new()
            {
                showMiddle = SystemInfo.graphicsMemorySize >= 4900 && SystemInfo.systemMemorySize > 7900
            };
            SavePrefs(data);
            return data;
        }

    }
}
