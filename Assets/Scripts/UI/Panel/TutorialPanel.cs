using Runner.Core;
using Runner.DataStudio.Asset;
using Runner.GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 新手引导UI面板: TutorialPanel
    /// Author: 单元琛 2022-8-26
    /// </summary>
    public class TutorialPanel : BasePanel
    {
        public class Option : PanelOption
        {
            public TutorialStruct tutorialStruct;
        }

        private TutorialPanel_Nodes nodes;
        protected override void OnStart()
        {
            nodes = rawNodes as TutorialPanel_Nodes;
            
            InitContent();
        }

        private void InitContent()
        {
            nodes.tutorial_txt.text = GetTutorialText((option as Option).tutorialStruct);
        }

        private string GetTutorialText(TutorialStruct info)
        {
            bool hasGamepad = Gamepad.current != null;
            if (info.gamepadid == 0)
            {
                return TableManager.Instance.GetText(info.textid);
            }
            else
            {
                return string.Format(TableManager.Instance.GetText(info.textid),
                hasGamepad ? TableManager.Instance.GetText(info.gamepadid) : TableManager.Instance.GetText(info.keyboardid));
            }
        }

    }

}
