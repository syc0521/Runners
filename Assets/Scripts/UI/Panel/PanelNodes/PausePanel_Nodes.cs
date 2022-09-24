using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runner.UI.Widget;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 暂停UI面板节点: PausePanel_Nodes
    /// Author: 单元琛 2022-8-11
    /// </summary>
    public class PausePanel_Nodes : BasePanelNodes
    {
        public GameObject backgroundImage;
        public GameObject rightImage;
        public TextMeshProUGUI progress_txt;
        public PauseSelectWidget retry_btn;
        public PauseSelectWidget selectLevel_btn;
        public PauseSelectWidget continue_btn;
        public HealthBarWidget progress_w;
    }
}
    
