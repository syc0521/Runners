using UnityEngine;

namespace Runner
{
    public abstract class PanelOption { }
}

namespace Runner.UI.Panel
{
    public abstract class BasePanelNodes : MonoBehaviour { }
    public abstract class BasePanel : MonoBehaviour
    {
        protected PanelOption option;
        private PanelEnum panelEnum;
        protected BasePanelNodes rawNodes;
        private void Start()
        {
            rawNodes = GetComponent<BasePanelNodes>();
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnStart() { }

        protected virtual void OnUpdate() { }

        protected virtual void OnPanelDestroy() { }

        protected virtual void OnShown() { }

        protected virtual void OnHidden() { }

        public void InitPanel(PanelEnum panelEnum, PanelOption option)
        {
            this.option = option;
            this.panelEnum = panelEnum;
        }

        protected void CloseSelf()
        {
            OnPanelDestroy();
            Core.UIManager.Instance.DestroyPanel(panelEnum);
        }

        public void ShowSelf()
        {
            gameObject.SetActive(true);
            OnShown();
        }

        public void HideSelf()
        {
            OnHidden();
            gameObject.SetActive(false);
        }

    }

}
