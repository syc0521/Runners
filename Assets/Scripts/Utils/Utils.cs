using System.Text.RegularExpressions;
using UnityEngine;

namespace Runner.Utils
{
    public class Utils
    {
        /// <summary>
        /// 自定义插值函数
        /// </summary>
        /// <param name="time">系统时间</param>
        /// <param name="timeRangeL">开始时间</param>
        /// <param name="timeRangeR">结束时间</param>
        /// <param name="posRangeL">开始位置</param>
        /// <param name="posRangeR">结束位置</param>
        /// <returns></returns>
        public static float Lerp(float time, float timeRangeL, float timeRangeR, float posRangeL, float posRangeR)
        {
            return Mathf.LerpUnclamped(posRangeL, posRangeR, (time - timeRangeL) / (timeRangeR - timeRangeL));
        }

        /// <summary>
        /// 正反斜杠转换
        /// </summary>
        /// <param name="s">输入字符串</param>
        /// <returns></returns>
        public static string ConvertSlash(string s)
        {
            return s.Replace("/", @"\");
        }

        public static string GetMusicPath(int id, int diff)
        {
            return Global.musicPath + string.Format("\\music{0:D4}\\{0:D4}_{1:D2}.txt", id, diff);
        }

        public static int GetMusicID(string path)
        {
            var actualPath = path[path.IndexOf("StreamingAssets\\GameData\\")..];
            Regex regex = new("[0-9]+");
            return int.Parse(regex.Match(actualPath).Value);
        }

        public static (string, string) GetMusicSourcePath(int id)
        {
            string acbPath = "GameData/musicsource" + string.Format("/music{0:D4}/music{0:D4}.acb", id);
            string awbPath = "GameData/musicsource" + string.Format("/music{0:D4}/music{0:D4}.awb", id);
            return (acbPath, awbPath);
        }

    }
}
