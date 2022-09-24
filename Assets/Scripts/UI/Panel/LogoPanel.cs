using Runner.Core;
using Runner.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CriWare;
using Runner.Utils;

namespace Runner.UI.Panel
{
    /// <summary>
    /// Logo显示UI面板: LogoPanel_Nodes
    /// Author: 单元琛 2022-8-13
    /// </summary>
    public class LogoPanel : BasePanel
    {

        private LogoPanel_Nodes nodes;
        protected override void OnStart()
        {
            nodes = rawNodes as LogoPanel_Nodes;
            InitContent();
        }

        private void InitContent()
        {
            StartCoroutine(ShowLogo());
        }

        private IEnumerator ShowLogo()
        {
            yield return new WaitForSeconds(0.5f);
            var seq = DOTween.Sequence()
                .Append(DOTween.To(() => 1.0f, x => nodes.rawImage_img.color = new Color(0, 0, 0, x), 0.1f, 1f))
                .AppendInterval(1.5f)
                .Append(DOTween.To(() => 0.1f, x => nodes.rawImage_img.color = new Color(0, 0, 0, x), 1.0f, 0.8f))
                .AppendInterval(1f)
                .AppendCallback(() => ShowEarphoneLogo())
                .Append(DOTween.To(() => 1.0f, x => nodes.rawImage_img.color = new Color(0, 0, 0, x), 0.1f, 1f))
                .AppendInterval(1.5f)
                .Append(DOTween.To(() => 0.1f, x => nodes.rawImage_img.color = new Color(0, 0, 0, x), 1.0f, 0.8f))
                .AppendInterval(1f)
                .AppendCallback(() => PlayTeamLogo())
                .AppendInterval(5.0f)
                .Append(DOTween.To(() => 1.0f, x => nodes.rawImage_img.color = new Color(0, 0, 0, x), 0.0f, 1.2f))
                .AppendCallback(() => StartMainPanel());
            seq.Play();

        }

        private void ShowEarphoneLogo()
        {
            nodes.criLogo.SetActive(false);
            nodes.earphoneLogo.SetActive(true);
        }

        private void PlayTeamLogo()
        {
            StartCoroutine(VibrateDuringMovie());
            nodes.criLogo.SetActive(false);
            nodes.earphoneLogo.SetActive(false);
            nodes.movieLogo_img.GetComponent<CriManaMovieControllerForUI>().Play();
        }

        private IEnumerator VibrateDuringMovie()
        {
            yield return new WaitForSeconds(0.4f);
            GamepadVibrateManager.Instance.Vibrate(VibrateEnum.ExtraLongLight);
        }

        private void StartMainPanel()
        {
            CloseSelf();
            UIManager.Instance.CreatePanel(PanelEnum.Loading, option = new LoadingPanel.Option { sceneEnum = SceneEnum.Start });
        }

    }

}
