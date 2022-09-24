using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runner.UI.Widget;

namespace Runner.UI.Panel
{
    /// <summary>
    /// ����UI���ڵ�: SettingPanel_Nodes
    /// Author: ��Ԫ� 2022-8-21
    /// </summary>
    public class SettingPanel_Nodes : BasePanelNodes
    {
        public Button back_btn;
        public Slider music_slider;
        public Slider se_slider;
        public Slider model_slider;
        public TextMeshProUGUI musicSliderValue_txt;
        public TextMeshProUGUI seSliderValue_txt;
        public TextMeshProUGUI modelSliderValue_txt;
        public Button reset_btn;
    }
}
    
