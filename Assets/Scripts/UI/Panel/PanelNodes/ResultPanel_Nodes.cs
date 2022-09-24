using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runner.UI.Widget;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 结算界面UI面板节点: ResultPanel_Nodes
    /// Author: 单元琛 2022-8-10
    /// </summary>
    public class ResultPanel_Nodes : BasePanelNodes
    {
        public Button retry_btn;
        public Button continue_btn;
        public TextMeshProUGUI grade_txt;
        public TextMeshProUGUI collection_txt;
        public TextMeshProUGUI score_txt;
    }
}
    
