using System.Collections;
using UnityEngine;
using Runner.DataStudio.Serialize;
using Runner.Core;
using UnityEngine.SceneManagement;
using Runner.UI.Panel;
using System;

namespace Runner.GamePlay
{
    public class GamePlayManager : Utils.Singleton<GamePlayManager>
    {
        public Camera mainCamera, grabCamera;
        public FumenData fumenData;
        public bool isPlaying = false;
        public float CurrentTime { get => AudioManager.Instance.GetMusicTime(AudioEnum.Track); }
        public static int MusicID = 2, FumenID = 0;
        public static int StartTime;
        public float TotalTime { get; private set; }
        public int Score { get; private set; }

        public OnValueChangedEventListener<float> playerHealth;
        public Action VictoryAction, RetryAction;
        private float totalCountTime = 0;
        public bool isStart = false;
        public PlayerController playerController;
        public GameObject middleScene;
        public float HealthMultiply { get => fumenData.healthMultiply; }
        public int CurrentBar 
        {
            get
            {
                if (isPlaying && fumenData != null)
                {
                    return AudioManager.Instance.GetMusicBar(AudioEnum.Track, fumenData.mainbpm, fumenData.barCount);
                }
                return 0;
            }
        }
        protected override void OnAwake()
        {
            GameManager.Instance.AddAppLeaveHandler(PauseGame);
        }
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (MusicID != 7)
            {
                middleScene.SetActive(SaveManager.Instance.GetPlayerData().showMiddle);
            }
            else
            {
                middleScene.SetActive(true);
            }
            ObjectManager.baseMoveSpeed = MusicID == 7 ? 3.5f : 7.1f;
            Debug.Log("GamePlayManager Start");
            fumenData = DataStudio.FumenReader.GetFumenData(Utils.Utils.GetMusicPath(MusicID, 0));
            if (AudioManager.Instance.isLoaded) return;
            TotalTime = AudioManager.Instance.LoadMusic(AudioEnum.Track, MusicID) / 1000.0f;
            totalCountTime = TotalTime;
            Debug.Log(TotalTime);
            UIManager.Instance.ChangeRenderMode(PanelEnum.Play, mainCamera);
            if (!GameManager.Instance.isEditor)
            {
                if (MusicID != 7)
                {
                    UIManager.Instance.CreatePanel(PanelEnum.Play);
                }
                else
                {
                    UIManager.Instance.CreatePanel(PanelEnum.Credits);
                    LoopSceneManager.Instance.Play();
                }
            }
            StartCoroutine(StartGame());
        }

        private void OnDestroy()
        {
            GameManager.Instance.RemoveAppLeaveHandler(PauseGame);
        }

        private IEnumerator StartGame()
        {
            isStart = true;
            RetryAction?.Invoke();
            Score = 0;
            ObjectManager.Instance.CurrentCollection = 0;
            totalCountTime = TotalTime;
            ObjectManager.Instance.StartGame(StartTime);
            Debug.Log((60000.0f / fumenData.mainbpm) / 1000.0f);
            yield return new WaitForSeconds(ObjectManager.baseMovetime);
            AudioManager.Instance.PlayMusic(AudioEnum.Track, StartTime);
            playerController.EnablePlayerController();
            isPlaying = true;
            if (MusicID != 7)
            {
                StartCoroutine(AutoDecreaseLife());
            }
        }

        private void Update()
        {
            if (GameManager.Instance.isEditor) return;
            if (isPlaying)
            {
                totalCountTime -= Time.deltaTime;
                if (totalCountTime <= 0)
                {
                    isStart = false;
                    isPlaying = false;
                    AudioManager.Instance.isLoaded = false;
                    if (MusicID != 7)
                    {
                        ShowResultPanel();
                    }
                    else
                    {
                        SceneManager.UnloadSceneAsync(SceneEnum.Gameplay.ToString());
                        GameManager.Instance.RestartGame();
                    }
                }
            }
        }

        public void ShowResultPanel()
        {
            VictoryAction?.Invoke();
            AudioManager.Instance.StopMusic(AudioEnum.Track);
            AudioManager.Instance.SetLabel("Result");
            AudioManager.Instance.PlayMusic(AudioEnum.BGM);
            StopCoroutine(AutoDecreaseLife());
            LoopSceneManager.Instance.Pause();
            UIManager.Instance.HidePanel(PanelEnum.Play);
            UIManager.Instance.CreatePanel(PanelEnum.Result);
        }

        public void Retry()
        {
            StartCoroutine(StartRetry());
        }

        private IEnumerator StartRetry()
        {
            UIManager.Instance.DestroyPanel(PanelEnum.Tutorial);
            ObjectManager.Instance.PlayTransitionSound();
            isPlaying = false;
            ObjectManager.Instance.CurrentCollection = 0;
            UIManager.Instance.HidePanel(PanelEnum.Play);
            StopCoroutine(AutoDecreaseLife());
            StopCoroutine(StartGame());
            UIManager.Instance.CreatePanel(PanelEnum.Transition, new TransitionPanel.Option { grabCamera = grabCamera });
            AudioManager.Instance.StopMusic(AudioEnum.Track);
            ObjectManager.Instance.StopGenerateNotes();
            UIManager.Instance.ShowPanel(PanelEnum.Play);
            yield return new WaitForSeconds(1f);
            LoopSceneManager.Instance.Play();
            StartCoroutine(StartGame());
            playerHealth.Value = 100;
        }

        public void PauseGame()
        {
            AudioManager.Instance.PauseMusic(AudioEnum.Track);
            UIManager.Instance.CreatePanel(PanelEnum.Pause);
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            AudioManager.Instance.ResumeMusic(AudioEnum.Track);
            Time.timeScale = 1;
        }

        public void ShowCredits()
        {
            StopMusic();
            MusicID = 7;
            Initialize();
        }

        public void ReturnMain()
        {
            StopMusic();
            SceneManager.UnloadSceneAsync(SceneEnum.Gameplay.ToString());
            UIManager.Instance.CreatePanel(PanelEnum.Loading, new LoadingPanel.Option { sceneEnum = SceneEnum.Start });
        }

        private void StopMusic()
        {
            isStart = false;
            isPlaying = false;
            ObjectManager.Instance.StopGenerateNotes();
            AudioManager.Instance.isLoaded = false;
            AudioManager.Instance.StopMusic(AudioEnum.Track);
            AudioManager.Instance.UnloadMusic(MusicID);
        }

        private IEnumerator AutoDecreaseLife()
        {
            if (!GameManager.Instance.isEditor)
            {
                while (true)
                {
                    if (isPlaying)
                    {
                        playerHealth.Value -= 1.45f * HealthMultiply;
                        yield return new WaitForSeconds((60000.0f / fumenData.mainbpm) / 1000.0f);
                    }
                    else
                    {
                        break;
                    }
                    yield return null;
                }
            }
        }

        public void AddLife(float life)
        {
            if (playerHealth == null) return;
            if (playerHealth.Value + life > 100.0f)
            {
                playerHealth.Value = 100.0f;
            }
            else if (playerHealth.Value + life <= 0.0f)
            {
                playerHealth.Value = 0.0f;
            }
            else
            {
                playerHealth.Value += life;
            }
        }

        public void AddScore(int score)
        {
            Score += score;
            if (Score <= 0)
            {
                Score = 0;
            }
        }

    }
}

