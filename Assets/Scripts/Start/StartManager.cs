using CriWare;
using Runner.Core;
using Runner.GamePlay;
using Runner.UI.Panel;
using Runner.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using DG.Tweening;
using System;

namespace Runner.Start
{
    /// <summary>
    /// 开始界面管理器: StartManager
    /// Author: 单元琛 2022-8-13
    /// </summary>
    public class StartManager : Singleton<StartManager>
    {
        private CriAtomSource audioSource, decideSource;
        public bool playAnim = true;
        public Cinemachine.CinemachineVirtualCamera mainCam, settingCam, selectCam;
        public Cinemachine.CinemachineVirtualCamera startCam1, startCam2, startCam3;
        public Volume volume;
        public Camera grabCamera;
        public Action animAction;
        public Animation startAnim;
        private bool isPlaying = false;
        private void Start()
        {
            audioSource = AudioManager.Instance.GetAudioSource(AudioEnum.BGM);
            decideSource = AudioManager.Instance.GetAudioSource(AudioEnum.Decide);
            decideSource.cueSheet = "Runner";

            if (GameManager.Instance.isFirstBoot)
            {
                startCam1.Priority = 100;
                UIManager.Instance.CreatePanel(PanelEnum.Title);
                GameManager.Instance.isFirstBoot = false;
                audioSource.cueSheet = "Runner";
                audioSource.cueName = "Title";
                decideSource.cueName = "Entry";
                StartCoroutine(PlayAnimation());
                isPlaying = true;
            }
            else
            {
                playAnim = false;
                UIManager.Instance.ShowPanel(PanelEnum.Main);
                UIManager.Instance.ShowPanel(PanelEnum.Select);
                playAnim = true;
                audioSource.cueName = "MainTrack";
                PlayBGM();
                animAction?.Invoke();
                decideSource.cueName = "Decide";
            }
            ObjectManager.moveSpeedMultiplier = 1;
            LoopSceneManager.Instance.Play();
        }

        private IEnumerator PlayAnimation()
        {
            yield return new WaitForSeconds(0.067f);
            startAnim.Play();
            yield return new WaitForSeconds(0.067f);
            audioSource.Play();
            yield return new WaitForSeconds(1.65f);
            animAction?.Invoke();
            UIManager.Instance.EnableUIAction();
        }

        public void OpenStartPanel()
        {
            StartCoroutine(StartPanel());
        }

        private IEnumerator StartPanel()
        {
            decideSource.Play();
            StopBGM();
            yield return new WaitForSeconds(1.5f);
            UIManager.Instance.CreatePanel(PanelEnum.Main);
            audioSource.cueName = "MainTrack";
            PlayBGM();
            animAction?.Invoke();
            decideSource.cueName = "Decide";
        }

        public void StartGame(int levelid, int fumenid)
        {
            GamePlayManager.MusicID = levelid;
            GamePlayManager.FumenID = fumenid;
            StopBGM();
            decideSource.Play();
            SceneManager.UnloadSceneAsync(SceneEnum.Start.ToString());
            UIManager.Instance.CreatePanel(PanelEnum.Loading, new LoadingPanel.Option { sceneEnum = SceneEnum.Gameplay });
        }

        public void ChangeCameraBlurOn()
        {
            if (volume.profile.TryGet(out DepthOfField dof))
            {
                DOTween.To(() => 1f, x => dof.focalLength.value = x, 300f, 0.8f).SetEase(Ease.OutQuad).SetUpdate(true);
            }
        }

        public void ChangeCameraBlurOff()
        {
            if (volume.profile.TryGet(out DepthOfField dof))
            {
                DOTween.To(() => 300f, x => dof.focalLength.value = x, 1f, 0.8f).SetEase(Ease.OutQuad).SetUpdate(true);
            }
        }

        public void QuitGame()
        {
            UIManager.Instance.CreatePanel(PanelEnum.Transition, new TransitionPanel.Option
            {
                grabCamera = grabCamera,
                hasOpen = false
            });
        }

        public void ChangeCamera(PanelEnum penum)
        {
            if (penum == PanelEnum.Select)
            {
                selectCam.Priority = 10;
                mainCam.Priority = 5;
                settingCam.Priority = 5;
            }
            else if (penum == PanelEnum.Settings)
            {
                selectCam.Priority = 5;
                mainCam.Priority = 5;
                settingCam.Priority = 10;
            }
            else
            {
                selectCam.Priority = 5;
                mainCam.Priority = 10;
                settingCam.Priority = 5;
            }
        }

        public void ChangeDecideSound(string s)
        {
            decideSource.cueName = s;
        }

        public void ChangeSelectSound(string s)
        {
            audioSource.cueName = s;
            StopBGM();
            PlayBGM();
        }

        public void StopBGM()
        {
            audioSource.Stop();
            isPlaying = false;
        }

        public void PlayBGM()
        {
            if (!isPlaying)
            {
                audioSource.Play();
                isPlaying = true;
            }
        }
    }

}
