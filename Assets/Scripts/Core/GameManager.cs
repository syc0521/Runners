using Runner.UI.Panel;
using Runner.Utils;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runner.Core
{
    /// <summary>
    /// 游戏管理器: GameManager
    /// Author: 单元琛 2022-8-9
    /// </summary>
    public class GameManager : Utils.Singleton<GameManager>
    {
        public bool isEditor;
#if UNITY_EDITOR
        public bool skipLogo;
#endif
        public GameObject[] startupList, editorStartupList;
        private Action OnAppFocus, OnAppLeave;
        [HideInInspector]
        public bool isFirstBoot = true;
        public GameObject canvas;
        public static bool HasGamepad => Gamepad.current != null;
        private void Start()
        {
            foreach (var item in startupList)
            {
                Instantiate(item);
            }
            if (isEditor)
            {
                foreach (var item in editorStartupList)
                {
                    Instantiate(item);
                }
                UIManager.Instance.CreatePanel(PanelEnum.Loading, new LoadingPanel.Option { sceneEnum = SceneEnum.FumenEditor });
            }
            else
            {
                Invoke(nameof(StartGame), 0.5f);
            }
        }

        private void OnDestroy()
        {
            GamepadVibrateManager.StopVibrate();
        }

        private void StartGame()
        {
#if UNITY_EDITOR
            if (skipLogo)
            {
                StartMainPanel();
            }
            else
#endif
            {
                UIManager.Instance.CreatePanel(PanelEnum.Logo);
            }
        }

        public void RestartGame()
        {
            isFirstBoot = true;
            UIManager.Instance.DestroyAllPanel();
            AudioManager.Instance.StopMusic(AudioEnum.BGM);
            UIManager.Instance.CreatePanel(PanelEnum.Loading, new LoadingPanel.Option { sceneEnum = SceneEnum.Start });
        }

        public void StartMainPanel()
        {
            UIManager.Instance.CreatePanel(PanelEnum.Loading, new LoadingPanel.Option { sceneEnum = SceneEnum.Start });
        }

        private void OnApplicationFocus(bool isFocus)
        {
#if !UNITY_EDITOR
            if (isFocus)
            {
                //回到游戏
                OnAppFocus?.Invoke();
            }
            else
            {
                //切入后台
                OnAppLeave?.Invoke();
            }
#endif
        }

        public void AddAppLeaveHandler(Action a)
        {
            OnAppLeave += a;
        }

        public void AddAppFocusHandler(Action a)
        {
            OnAppFocus += a;
        }

        public void RemoveAppLeaveHandler(Action a)
        {
            OnAppLeave -= a;
        }

        public void RemoveAppFocusHandler(Action a)
        {
            OnAppFocus -= a;
        }

        public void QuitProgram()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else   
            Application.Quit();
#endif
        }

    }

}
