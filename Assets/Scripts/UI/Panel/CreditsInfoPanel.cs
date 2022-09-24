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
    /// 制作人员名单信息UI面板: CreditsInfoPanel
    /// Author: 单元琛 2022-8-10
    /// </summary>
    public class CreditsInfoPanel : BasePanel
    {
        public class Option : PanelOption
        {
            public int id;
        }

        private CreditsInfoPanel_Nodes nodes;
        private string title, desc;
        private int id;
        protected override void OnStart()
        {
            nodes = rawNodes as CreditsInfoPanel_Nodes;
            InitData();
            InitContent();
        }
        
        private void InitData()
        {
            id = (option as Option).id;
            var info = TableManager.Instance.GetCreditsInfo((option as Option).id);
            title = TableManager.Instance.GetText(info.titleid);
            desc = TableManager.Instance.GetText(info.descid);
        }
        private void InitContent()
        {
            nodes.title.text = title;
            nodes.desc.text = desc;
            StartCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            DOTween.To(() => 0.0f, x => nodes.bg_img.color = new Color(1, 1, 1, x), 1f, 1f).SetUpdate(true);
            DOTween.To(() => 0.0f, x => nodes.title.color = new Color(1, 1, 1, x), 1f, 1f).SetUpdate(true);
            DOTween.To(() => 0.0f, x => nodes.desc.color = new Color(1, 1, 1, x), 1f, 1f).SetUpdate(true);
            yield return new WaitForSeconds(id == 15 ? 5f : 3f);
            DOTween.To(() => 1f, x => nodes.bg_img.color = new Color(1, 1, 1, x), 0f, 1f).SetUpdate(true);
            DOTween.To(() => 1f, x => nodes.title.color = new Color(1, 1, 1, x), 0f, 1f).SetUpdate(true);
            DOTween.To(() => 1f, x => nodes.desc.color = new Color(1, 1, 1, x), 0f, 1f).SetUpdate(true);
            yield return new WaitForSeconds(1.5f);
            CloseSelf();
        }

    }

}
