using DG.Tweening;
using Runner.Core;
using Runner.GamePlay;
using Runner.Start;
using Runner.UI.Widget;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 开始界面UI面板: StartPanel
    /// Author: 单元琛 2022-8-10
    /// </summary>
    public class StartPanel : BasePanel
    {
        private StartPanel_Nodes nodes;
        private PanelEnum openedPanel;
        protected override void OnStart()
        {
            nodes = rawNodes as StartPanel_Nodes;
            InitContent();
        }

        private void InitContent()
        {
            AudioManager.Instance.SetLabel("Title");
            nodes.start_btn.Initialize();
            nodes.setting_btn.Initialize();
            nodes.quit_btn.Initialize();
            nodes.start_btn.AddListener(StartSelect);
            nodes.setting_btn.AddListener(OpenSetting);
            nodes.quit_btn.AddListener(QuitGame);
            nodes.start_btn.SelectThis();
            StartManager.Instance.animAction += PlayMainAnimation;
        }

        protected override void OnHidden()
        {
            nodes.start_btn.RemoveAllListeners();
            nodes.setting_btn.RemoveAllListeners();
            nodes.quit_btn.RemoveAllListeners();
        }

        protected override void OnPanelDestroy()
        {
            StartManager.Instance.animAction += PlayMainAnimation;
            nodes.start_btn.RemoveAllListeners();
            nodes.setting_btn.RemoveAllListeners();
            nodes.quit_btn.RemoveAllListeners();
        }

        private void StartSelect()
        {
            openedPanel = PanelEnum.Select;
            StartManager.Instance.ChangeCamera(PanelEnum.Select);
            var seq = DOTween.Sequence().
                Append(gameObject.transform.DOLocalMoveY(1080, .5f)).
                AppendCallback(() =>
                {
                    HideSelf();
                    UIManager.Instance.CreatePanel(PanelEnum.Select);
                    AudioManager.Instance.SetLabel("Select");
                });
            seq.Play();
        }

        protected override void OnShown()
        {
            base.OnShown();
            nodes.start_btn.AddListener(StartSelect);
            nodes.setting_btn.AddListener(OpenSetting);
            nodes.quit_btn.AddListener(QuitGame);
            if (StartManager.Instance.playAnim)
            {
                gameObject.transform.DOLocalMoveX(0, .5f);
                gameObject.transform.DOLocalMoveY(0, .5f);
            }
            if (openedPanel == PanelEnum.Settings)
            {
                nodes.start_btn.UnSelectThis();
                nodes.setting_btn.SelectThis();
            }
            else if (openedPanel == PanelEnum.Select)
            {
                nodes.start_btn.SelectThis();
                nodes.setting_btn.UnSelectThis();
            }
        }

        private void OpenSetting()
        {
            openedPanel = PanelEnum.Settings;
            StartManager.Instance.ChangeCamera(PanelEnum.Settings);
            var seq = DOTween.Sequence().
                Append(gameObject.transform.DOLocalMoveX(-1920, .5f)).
                AppendCallback(() =>
                {
                    HideSelf();
                    UIManager.Instance.CreatePanel(PanelEnum.Settings);
                    AudioManager.Instance.SetLabel("Settings");
                });
            seq.Play();
        }
        private void QuitGame()
        {
            UIManager.Instance.DisableUIAction();
            var seq = DOTween.Sequence().
                Append(gameObject.transform.DOLocalMoveX(1920, .5f)).
                AppendCallback(() => StartManager.Instance.QuitGame()).
                AppendInterval(1.1f).
                AppendCallback(() => GameManager.Instance.QuitProgram());
            seq.Play();
        }

        public void PlayMainAnimation()
        {
            foreach (PropertyInfo p in nodes.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(IWithBeatWidget))
                {
                    p.GetType().GetMethod("PlayUIAnimation")?.Invoke(this, new object[] { p });
                }
            }
        }

    }

}
