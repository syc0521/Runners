using System.Collections.Generic;
using UnityEngine;
using Runner.Utils;
using Runner.UI.Panel;

namespace Runner.Core
{
    /// <summary>
    /// UI管理器: UIManager
    /// Author: 单元琛 2022-8-9
    /// </summary>
    public partial class UIManager : Singleton<UIManager>
    {
        [System.Serializable]
        public struct UIPanelStruct
        {
            public PanelEnum id;
            public GameObject panel;
            public int renderDistance;
        }

        public UIPanelStruct[] panels;
        private GameObject canvas;

        private Dictionary<PanelEnum, UIPanelStruct> panelDictionary;
        private Dictionary<PanelEnum, GameObject> openedPanel;
        private PlayerControls playerControls;

        protected override void OnAwake()
        {
            playerControls = new();
            panelDictionary = new();
            openedPanel = new();
            canvas = GameManager.Instance.canvas;
            for (int i = 0; i < panels.Length; i++)
            {
                if (!panelDictionary.ContainsKey(panels[i].id))
                {
                    panelDictionary.Add(panels[i].id, panels[i]);
                }
            }
        }

        public void CreatePanel(PanelEnum panelEnum, PanelOption option = null)
        {
            if (openedPanel.ContainsKey(panelEnum)) return;

            var obj = Instantiate(panelDictionary[panelEnum].panel);
            obj.transform.SetParent(canvas.transform, false);
            var panel = obj.GetComponent<BasePanel>();
            panel.InitPanel(panelEnum, option);
            openedPanel.Add(panelEnum, obj);
        }

        public void DestroyPanel(PanelEnum panelEnum)
        {
            if (openedPanel.ContainsKey(panelEnum))
            {
                Destroy(openedPanel[panelEnum]);
                openedPanel.Remove(panelEnum);
            }
        }

        public void DestroyAllPanel()
        {
            foreach (var item in openedPanel)
            {
                Destroy(item.Value);
            }
            openedPanel.Clear();
        }

        public void ChangeRenderMode(PanelEnum panelEnum, Camera camera)
        {
            var cvs = canvas.GetComponent<Canvas>();
            cvs.renderMode = RenderMode.ScreenSpaceCamera;
            cvs.worldCamera = camera;
            cvs.planeDistance = panelDictionary[panelEnum].renderDistance;
        }

        public void ShowPanel(PanelEnum panelEnum)
        {
            var panel = openedPanel[panelEnum];
            panel.GetComponent<BasePanel>().ShowSelf();
        }

        public void HidePanel(PanelEnum panelEnum)
        {
            var panel = openedPanel[panelEnum];
            panel.GetComponent<BasePanel>().HideSelf();
        }

    }

}
