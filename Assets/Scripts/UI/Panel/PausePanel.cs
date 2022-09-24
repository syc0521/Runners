using Runner.Core;
using Runner.GamePlay;
using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Runner.UI.Panel
{
    /// <summary>
    /// ÔÝÍ£UIÃæ°å: PausePanel
    /// Author: µ¥Ôªè¡ 2022-8-11
    /// </summary>
    public class PausePanel : BasePanel
    {
        private float progress;
        private PausePanel_Nodes nodes;
        private Animator animator;
        protected override void OnStart()
        {
            animator = GetComponent<Animator>();
            nodes = rawNodes as PausePanel_Nodes;
            UIManager.Instance.HidePanel(PanelEnum.Play);
            InitData();
            InitContent();
        }

        private void InitData()
        {
            progress = GamePlayManager.Instance.CurrentTime / GamePlayManager.Instance.TotalTime;
        }

        private void InitContent()
        {
            nodes.continue_btn.Initialize();
            nodes.retry_btn.Initialize();
            nodes.selectLevel_btn.Initialize();
            nodes.continue_btn.AddListener(Continue);
            nodes.retry_btn.AddListener(RetryGame);
            nodes.selectLevel_btn.AddListener(ReturnMain);
            nodes.progress_w.SetHealth(progress);
            nodes.continue_btn.SelectThis();
        }

        protected override void OnPanelDestroy()
        {
            nodes.continue_btn.RemoveAllListeners();
            nodes.retry_btn.RemoveAllListeners();
            nodes.selectLevel_btn.RemoveAllListeners();
        }

        private void RetryGame()
        {
            UIManager.Instance.DestroyPanel(PanelEnum.Tutorial);
            StartCoroutine(CloseSelfPanel(() =>
            {
                GamePlayManager.Instance.ResumeGame();
                GamePlayManager.Instance.Retry();
                UIManager.Instance.ShowPanel(PanelEnum.Play);
            }));
        }

        private void Continue()
        {
            StartCoroutine(CloseSelfPanel(() =>
            {
                UIManager.Instance.ShowPanel(PanelEnum.Play);
                GamePlayManager.Instance.ResumeGame();
            }));
        }

        private void ReturnMain()
        {
            GamePlayManager.Instance.ResumeGame();
            GamePlayManager.Instance.ReturnMain();
            UIManager.Instance.DestroyPanel(PanelEnum.Tutorial);
            UIManager.Instance.DestroyPanel(PanelEnum.Play);
            CloseSelf();
        }

        private IEnumerator CloseSelfPanel(Action action)
        {
            animator.Play("ResumeAnimation");
            yield return new WaitForSecondsRealtime(.5f);
            action?.Invoke();
            CloseSelf();
        }

    }

}
