using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.DataStudio
{
    public class FumenReader
    {
        
        public static Serialize.FumenData GetFumenData(string path)
        {
            Serialize.FumenData fumenData = new();
            var text = System.IO.File.ReadAllText(path);
            var lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            int headeridx = 0;
            int compidx = Array.IndexOf(lines, "[COMPOSITION]");
            int collectionidx = Array.IndexOf(lines, "[COLLECTION]");
            int layeridx = Array.IndexOf(lines, "[LAYER]");
            int obstacleidx = Array.IndexOf(lines, "[OBSTACLE]");

            // 读取Header部分
            for (int i = headeridx + 1; i < compidx; i++)
            {
                if (LineIsNotNull(lines[i]))
                {
                    var stringLine = lines[i].Split('\t');
                    switch (stringLine[0])
                    {
                        case "BPM_DEF":
                            fumenData.mainbpm = float.Parse(stringLine[1]);
                            break;
                        case "MET_DEF":
                            fumenData.barCount = int.Parse(stringLine[1]);
                            fumenData.beatCount = int.Parse(stringLine[2]);
                            break;
                        case "VERSION":
                            fumenData.version = new(uint.Parse(stringLine[1]), uint.Parse(stringLine[2]));
                            break;
                        case "HEALTH":
                            fumenData.healthMultiply = int.Parse(stringLine[1]) / 10.0f;
                            break;
                        default:
                            break;
                    }
                }
            }

            // 读取composition部分
            for (int i = compidx + 1; i < layeridx; i++)
            {
                if (LineIsNotNull(lines[i]))
                {
                    var stringLine = lines[i].Split('\t');
                    uint met = uint.Parse(stringLine[1]);
                    uint submet = uint.Parse(stringLine[2]);
                    uint para = uint.Parse(stringLine[3]);
                    Serialize.Composition note = new();
                    switch (stringLine[0])
                    {
                        case "FLY":
                            note = new Serialize.FlyComposition(met, submet, para);
                            break;
                        case "BOS":
                            note = new Serialize.BossComposition(met, submet, para);
                            break;
                        case "TUT":
                            note = new Serialize.TutorialComposition(met, submet, para);
                            break;
                        case "CRE":
                            note = new Serialize.CreditComposition(met, submet, para);
                            break;
                        default:
                            break;
                    }
                    float bpm = fumenData.mainbpm;
                    note.time = (uint)((60000.0f / bpm * 4.0f) * (met + submet / 384.0f)) / 1000.0f;
                    fumenData.compositions.Add(note);
                }
            }

            // 读取Note部分
            for (int i = collectionidx + 1; i < obstacleidx; i++)
            {
                if (LineIsNotNull(lines[i]))
                {
                    var stringLine = lines[i].Split('\t');
                    uint met = uint.Parse(stringLine[1]);
                    uint submet = uint.Parse(stringLine[2]);
                    uint pos = uint.Parse(stringLine[3]);
                    Serialize.Collection note = new();
                    switch (stringLine[0])
                    {
                        case "BRK":
                            note = new Serialize.Brick(met, submet, pos);
                            break;
                        case "DMD":
                            break;
                        default:
                            break;
                    }
                    float bpm = fumenData.mainbpm;
                    note.time = (uint)((60000.0f / bpm * 4.0f) * (met + submet / 384.0f)) / 1000.0f;
                    fumenData.collections.Add(note);
                }
            }

            // 读取障碍部分
            for (int i = obstacleidx + 1; i < lines.Length; i++)
            {
                if (LineIsNotNull(lines[i]))
                {
                    var stringLine = lines[i].Split('\t');
                    uint met = uint.Parse(stringLine[1]);
                    uint submet = uint.Parse(stringLine[2]);
                    uint pos = uint.Parse(stringLine[3]);
                    Serialize.Obstacle note = new();
                    switch (stringLine[0])
                    {
                        case "JMP":
                            note = new Serialize.JumpObstacle(met, submet, pos);
                            break;
                        case "SLD":
                            note = new Serialize.SlideObstacle(met, submet, pos);
                            break;
                        case "KIK":
                            note = new Serialize.KickObstacle(met, submet, pos);
                            break;
                        case "LTN":
                            note = new Serialize.LanternObstacle(met, submet, pos);
                            break;
                        case "MIS":
                            note = new Serialize.MissileObstacle(met, submet, pos);
                            break;
                        case "REV":
                            note = new Serialize.ReverseObstacle(met, submet, pos);
                            break;
                        default:
                            break;
                    }
                    float bpm = fumenData.mainbpm;
                    note.time = (uint)((60000.0f / bpm * 4.0f) * (met + submet / 384.0f)) / 1000.0f;
                    fumenData.obstacles.Add(note);
                }
            }

            fumenData.collections.Sort();
            return fumenData;
        }

        public static bool LineIsNotNull(string line)
        {
            return line != null || !line.Equals("");
        }

        public static Serialize.LayerType GetLayerType(string str)
        {
            Serialize.LayerType type = Serialize.LayerType.Null;
            switch (str)
            {
                case "LYS":
                    type = Serialize.LayerType.Start;
                    break;
                case "LYR":
                    type = Serialize.LayerType.Normal;
                    break;
                case "LYE":
                    type = Serialize.LayerType.End;
                    break;
                default:
                    break;
            }
            return type;
        }
    }

}
