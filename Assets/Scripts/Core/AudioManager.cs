using System.Collections.Generic;
using UnityEngine;
using Runner.Utils;
using CriWare;
using static UnityEngine.Rendering.DebugUI;

namespace Runner.Core
{
    /// <summary>
    /// 音频管理器: AudioManager
    /// Author: 单元琛 2022-8-9
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        private Dictionary<string, CriAtomSource> musicList;
        public bool isLoaded = false;
        public float MusicVolume { get; private set; }
        public float SEVolume { get; private set; }
        protected override void OnAwake()
        {
            musicList = new();
        }

        void Start()
        {
            AddAudioSource(AudioEnum.Track);
            AddAudioSource(AudioEnum.BGM);
            AddAudioSource(AudioEnum.Decide);
        }

        public void AddAudioSource(AudioEnum name)
        {
            GameObject go = new()
            {
                name = name.ToString(),
            };
            go.transform.parent = gameObject.transform;
            var music = go.AddComponent<CriAtomSource>();
            musicList.Add(name.ToString(), music);
        }

        public CriAtomSource GetAudioSource(string name)
        {
            if (musicList == null || musicList.Count == 0) return null;
            return musicList[name];
        }

        public CriAtomSource GetAudioSource(AudioEnum name)
        {
            if (musicList == null || musicList.Count == 0) return null;
            return musicList[name.ToString()];
        }

        public long LoadMusic(AudioEnum name, int id)
        {
            isLoaded = true;
            return MusicHandler.LoadMusic(id, GetAudioSource(name));
        }

        public void UnloadMusic(int id)
        {
            MusicHandler.UnloadMusic(id);
        }

        public void StopMusic(AudioEnum name)
        {
            MusicHandler.StopMusic(GetAudioSource(name.ToString()));
        }

        public void PlayMusic(AudioEnum name)
        {
            MusicHandler.PlayMusic(GetAudioSource(name.ToString()));
        }

        public void PlayMusic(AudioEnum name, float time)
        {
            MusicHandler.PlayMusic(GetAudioSource(name.ToString()), time);
        }

        public int GetMusicBar(AudioEnum name, float bpm, int barCount)
        {
            return MusicHandler.GetMusicBar(GetAudioSource(name.ToString()), bpm, barCount);
        }

        public float GetMusicTime(AudioEnum name)
        {
            return MusicHandler.GetMusicTime(GetAudioSource(name.ToString()));
        }

        public void PauseMusic(AudioEnum name)
        {
            MusicHandler.PauseMusic(GetAudioSource(name.ToString()));
        }

        public void ResumeMusic(AudioEnum name)
        {
            MusicHandler.ResumeMusic(GetAudioSource(name.ToString()));
        }

        public void SetLabel(string label)
        {
            var source = GetAudioSource(AudioEnum.BGM).player;
            source.UnsetSelectorLabel("State");
            source.UpdateAll();
            source.SetSelectorLabel("State", label);
            source.UpdateAll();
        }

        public void SetAudioVolume(AudioEnum name, float volume)
        {
            MusicHandler.SetVolume(GetAudioSource(name), volume);
        }

        public void SetMusicVolume(float volume)
        {
            SetAudioVolume(AudioEnum.BGM, volume);
            SetAudioVolume(AudioEnum.Decide, volume);
            SetAudioVolume(AudioEnum.Track, volume);
       }

    }

}
