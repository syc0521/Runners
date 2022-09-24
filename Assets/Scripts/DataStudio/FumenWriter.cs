using Runner.DataStudio.Serialize;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Runner.DataStudio
{
    public class FumenWriter
    {
        public static bool WriteFumenData(Serialize.FumenData fumenData, string path)
        {
            StringBuilder sb = new();
            sb.Append("[HEADER]\n" +
                $"VERSION\t{fumenData.version.major}\t{fumenData.version.minor}\n" +
                $"BPM_DEF\t{fumenData.mainbpm:F3}\n" +
                $"MET_DEF\t{fumenData.barCount}\t{fumenData.beatCount}\n" +
                "RESOLUTION\t384\n");
            if (fumenData.version.Equals(Version.Version10))
            {
                sb.Append("\n[COMPOSITION]");
            }
            else if (fumenData.version.Equals(Version.Version11))
            {
                sb.Append($"HEALTH\t{Mathf.CeilToInt(fumenData.healthMultiply * 10.0f)}" +
                    "\n\n[COMPOSITION]");
            }
            fumenData.compositions.Sort();
            foreach (var item in fumenData.compositions)
            {
                switch (item.compositionType)
                {
                    case CompositionType.Fly:
                        sb.Append($"\nFLY\t{item.met}\t{item.submet}\t{(int)item.para}");
                        break;
                    case CompositionType.Boss:
                        sb.Append($"\nBOS\t{item.met}\t{item.submet}\t{(int)item.para}");
                        break;
                    case CompositionType.Tutorial:
                        sb.Append($"\nTUT\t{item.met}\t{item.submet}\t{(int)item.para}");
                        break;
                    case CompositionType.Credit:
                        sb.Append($"\nCRE\t{item.met}\t{item.submet}\t{(int)item.para}");
                        break;
                    default:
                        break;
                }
            }
            sb.Append("\n\n[LAYER]\n\n" +
                "[COLLECTION]");
            fumenData.collections.Sort();
            fumenData.obstacles.Sort();
            foreach (var item in fumenData.collections)
            {
                sb.Append($"\nBRK\t{item.met}\t{item.submet}\t{(int)item.positionType}");
            }
            sb.Append("\n\n[OBSTACLE]");
            foreach (var item in fumenData.obstacles)
            {
                switch (item.type)
                {
                    case ObstacleType.Jump:
                        sb.Append($"\nJMP\t{item.met}\t{item.submet}\t{(int)item.positionType}");
                        break;
                    case ObstacleType.Slide:
                        sb.Append($"\nSLD\t{item.met}\t{item.submet}\t{(int)item.positionType}");
                        break;
                    case ObstacleType.Kick:
                        sb.Append($"\nKIK\t{item.met}\t{item.submet}\t{(int)item.positionType}");
                        break;
                    case ObstacleType.Lantern:
                        sb.Append($"\nLTN\t{item.met}\t{item.submet}\t{(int)item.positionType}");
                        break;
                    case ObstacleType.Missile:
                        sb.Append($"\nMIS\t{item.met}\t{item.submet}\t{(int)item.positionType}");
                        break;
                    case ObstacleType.Reverse:
                        sb.Append($"\nREV\t{item.met}\t{item.submet}\t{(int)item.positionType}");
                        break;
                    default:
                        break;
                }

            }
            Debug.Log(sb.ToString());
            System.IO.File.WriteAllText(path, sb.ToString());
            return true;
        }
    }
}
