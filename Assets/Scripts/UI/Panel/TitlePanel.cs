using Runner.Core;
using Runner.Start;
using Runner.UI.Widget;
using System.Reflection;
using static UnityEngine.InputSystem.InputAction;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 标题UI面板: TitlePanel
    /// Author: 单元琛 2022-9-2
    /// </summary>
    public class TitlePanel : BasePanel
    {
        private TitlePanel_Nodes nodes;
        protected override void OnStart()
        {
            nodes = rawNodes as TitlePanel_Nodes;
            InitContent();
        }

        private void InitContent()
        {
            nodes.logo.gameObject.SetActive(false);
            nodes.main_txt.text = string.Format(TableManager.Instance.GetText(0),
                GameManager.HasGamepad ? TableManager.Instance.GetText(1004) :
                TableManager.Instance.GetText(1000));
            StartManager.Instance.animAction += PlayAnimation;
            UIManager.Instance.AddConfirmAction(StartGame);
            UIManager.Instance.AddAnyKeyAction(StartGame);
        }

        protected override void OnPanelDestroy()
        {
            UIManager.Instance.RemoveConfirmAction(StartGame);
            UIManager.Instance.RemoveAnyKeyAction(StartGame);
            StartManager.Instance.animAction -= PlayAnimation;
            base.OnPanelDestroy();
        }

        private void StartGame()
        {
            StartManager.Instance.OpenStartPanel();
            CloseSelf();
        }

        private void StartGame(CallbackContext ctx) => StartGame();

        public void PlayAnimation()
        {
            nodes.logo.gameObject.SetActive(true);
            nodes.main_txt.PlayUIAnimation();
        }

    }

}
