using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 谱面编辑器UI面板节点: FumenEditorPanel_Nodes
    /// Author: 单元琛 2022-8-6
    /// </summary>
    public class FumenEditorPanel_Nodes : BasePanelNodes
    {
        public RawImage background_rawimage;
        public Button open_btn;
        public Button preview_btn;
        public Button save_btn;
        public Button composition_btn;
        public Button layer_btn;
        public Slider barSlider_slider;
        public Button play_btn;
        public Button pause_btn;
        public TMP_InputField bar_input;
        public TMP_Dropdown division_dropdown;
        public Button delete_btn;
        public Button brick_btn;
        public Button lantern_btn;
        public Button missile_btn;
        public Button reverse_btn;
        public Button jump_btn;
        public Button slide_btn;
        public Button kick_btn;
        public Button quit_btn;
        public GameObject compositionWindow;
        public GameObject compositionContent_list;
        public Button comp_add_btn;
        public Button comp_edit_btn;
        public Button comp_delete_btn;
        public Button comp_close_btn;
        public GameObject edit_panel;
        public TMP_Dropdown comp_dropdown;
        public TMP_InputField met_input;
        public TMP_InputField submet_input;
        public TMP_InputField para_input;
        public Button compConfirm_btn;
    }

}
