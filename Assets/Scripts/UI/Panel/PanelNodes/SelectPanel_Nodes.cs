using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runner.UI.Widget;

namespace Runner.UI.Panel
{
    /// <summary>
    /// ѡ�ؽ���UI���ڵ�: SelectPanel_Nodes
    /// Author: ��Ԫ� 2022-8-21
    /// </summary>
    public class SelectPanel_Nodes : BasePanelNodes
    {
        public List<LevelSelectWidget> levelButtons_w = new();
        public Button back_btn;
        public Button freeMode_btn;
        public TextMeshProUGUI songTitle_txt;
        public TextMeshProUGUI songDesc_txt;
        public TextMeshProUGUI collection_txt;
        public TextMeshProUGUI score_txt;
        public GameObject confirmArea;
        public Button start_btn;
        public Button background_btn;
    }
}
    
