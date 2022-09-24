using Runner.Core;
using Runner.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 谱面编辑器测试用UI面板: TestPanel
    /// Author: 单元琛 2022-8-10
    /// </summary>
    public class TestPanel : BasePanel
    {
        public class TestPanelOption : PanelOption
        {
            public int startTime;
        }

        private TestPanel_Nodes nodes;
        protected override void OnStart()
        {
            nodes = rawNodes as TestPanel_Nodes;
            GamePlayManager.StartTime = (option as TestPanelOption).startTime;
            InitContent();
        }

        protected override void OnUpdate()
        {
            if (GamePlayManager.Instance == null) return;

            var curBar = GamePlayManager.Instance.CurrentBar;
            nodes.bar_txt.text = curBar <= 0 ? "Loading......" : curBar.ToString();
        }

        private void InitContent()
        {
            nodes.back_btn.onClick.AddListener(() => QuitPreview());
        }

        private void QuitPreview()
        {
            CloseSelf();
            SceneManager.UnloadSceneAsync(SceneEnum.Gameplay.ToString());
            UIManager.Instance.ShowPanel(PanelEnum.FumenEditor);
            UIManager.Instance.DestroyPanel(PanelEnum.Tutorial);
            ObjectManager.Instance.StopGenerateNotes();
            AudioManager.Instance.StopMusic(AudioEnum.Track);
        }

    }

}
