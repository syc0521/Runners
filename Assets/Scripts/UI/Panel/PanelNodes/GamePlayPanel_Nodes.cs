using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runner.UI.Widget;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 游戏界面UI面板节点: GamePlay_Nodes
    /// Author: 单元琛 2022-8-11
    /// </summary>
    public class GamePlayPanel_Nodes : BasePanelNodes
    {
        public Button pause_btn;
        public HealthBarWidget healthbar_w;
        public TextMeshProUGUI score_txt;
    }
}
    
