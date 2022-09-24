using DG.Tweening;
using Runner.Core;
using Runner.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 制作人员名单UI面板: TestPanel
    /// Author: 单元琛 2022-8-27
    /// </summary>
    public class CreditsPanel : BasePanel
    {
        private CreditsPanel_Nodes nodes;
        protected override void OnStart()
        {
            nodes = rawNodes as CreditsPanel_Nodes;
            InitContent();
        }

        private void InitContent()
        {
            StartCoroutine(StartAnim());
        }

        private IEnumerator StartAnim()
        {
            yield return new WaitForSeconds(5.5f);
            DOTween.To(() => 0.0f, x => nodes.logo_img.color = new Color(1, 1, 1, x), 1f, 1f).SetUpdate(true);
            yield return new WaitForSeconds(3f);
            DOTween.To(() => 1.0f, x => nodes.logo_img.color = new Color(1, 1, 1, x), 0f, 1f).SetUpdate(true);
        }

    }

}
