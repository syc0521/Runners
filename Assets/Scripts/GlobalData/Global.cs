using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    public class Global : MonoBehaviour
    {
        public static readonly string streamingAssetsPath = Utils.Utils.ConvertSlash(Application.streamingAssetsPath);
        public static readonly string gameDataPath = streamingAssetsPath + "\\GameData";
        public static readonly string optionPath = streamingAssetsPath + "\\option";
        public static readonly string musicPath = gameDataPath + "\\music";
        public static readonly string musicSourcePath = gameDataPath + "\\musicsource";
        public static readonly string savePath = Application.persistentDataPath + "/save.json";
    }

}
