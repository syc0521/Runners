using CriWare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Utils
{
    public static class MusicHandler
    {
        public static long LoadMusic(int id, CriAtomSource music)
        {
            var musicSourcePath = Utils.GetMusicSourcePath(id);
            CriAtom.AddCueSheet(string.Format("music{0:D4}", id), musicSourcePath.Item1, musicSourcePath.Item2, null);
            music.cueName = "track";
            music.cueSheet = string.Format("music{0:D4}", id);
            CriAtom.GetCueSheet(string.Format("music{0:D4}", id)).acb.GetCueInfoByIndex(0, out CriAtomEx.CueInfo info);
            return info.length;
        }

        public static void UnloadMusic(int id)
        {
            CriAtom.RemoveCueSheet(string.Format("music{0:D4}", id));
        }

        public static void PlayMusic(CriAtomSource music) => music.Play();
        public static void PlayMusic(CriAtomSource music, float time)
        {
            music.startTime = (int)time;
            music.Play();
        }

        public static void StopMusic(CriAtomSource music) => music.Stop();
        public static void PauseMusic(CriAtomSource music) => music.Pause(true);
        public static void ResumeMusic(CriAtomSource music) => music.Pause(false);
        public static float GetMusicTime(CriAtomSource music) => music.time / 1000.0f;
        public static int GetMusicBar(CriAtomSource music, float bpm, int barCount)
        {
            float timePerBar = GetTimePerBar(bpm, barCount);
            return Mathf.CeilToInt(music.time / timePerBar) - 1;
        }
        public static float ConvertBarToTime(int bar, float bpm, int barCount)
        {
            float timePerBar = GetTimePerBar(bpm, barCount);
            return timePerBar * bar;
        }
        public static int GetMusicTotalBar(long length, float bpm, int barCount)
        {
            float timePerBar = GetTimePerBar(bpm, barCount);
            return Mathf.CeilToInt(length / timePerBar);
        }
        private static float GetTimePerBar(float bpm, int barCount) => 60000.0f / bpm * barCount;

        public static void SetVolume(CriAtomSource music, float volume)
        {
            music.volume = volume;
        }
    }
}