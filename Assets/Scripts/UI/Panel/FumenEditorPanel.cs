using Runner.Core;
using Runner.DataStudio.Serialize;
using Runner.FumenEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.UI.Panel
{
    public class FumenEditorPanel : BasePanel
    {
        private float[] editorViewportPosition;
        private FumenEditorPanel_Nodes nodes;
        public GameObject compositionWidget;
        private List<GameObject> compositionWidgetList;

        protected override void OnStart()
        {
            nodes = rawNodes as FumenEditorPanel_Nodes;
            InitData();
            InitContent();
        }

        protected override void OnShown()
        {
            FumenEditorManager.Instance.ChangeRenderCamera();
            FumenEditorManager.Instance.GenerateItems();
            ChangeDivision(1);
        }

        protected override void OnHidden()
        {
            FumenEditorManager.Instance.ClearItems();
        }

        protected override void OnPanelDestroy()
        {
            FumenEditorManager.Instance.ClearItems();
            LineController.Instance.ClearAllLines();

            nodes.open_btn.onClick.RemoveAllListeners();
            nodes.save_btn.onClick.RemoveAllListeners();
            nodes.play_btn.onClick.RemoveAllListeners();
            nodes.pause_btn.onClick.RemoveAllListeners();
            nodes.brick_btn.onClick.RemoveAllListeners();
            nodes.jump_btn.onClick.RemoveAllListeners();
            nodes.slide_btn.onClick.RemoveAllListeners();
            nodes.kick_btn.onClick.RemoveAllListeners();
            nodes.lantern_btn.onClick.RemoveAllListeners();
            nodes.delete_btn.onClick.RemoveAllListeners();
            nodes.bar_input.onEndEdit.RemoveAllListeners();
            nodes.division_dropdown.onValueChanged.RemoveAllListeners();
            nodes.barSlider_slider.onValueChanged.RemoveAllListeners();
            nodes.preview_btn.onClick.RemoveAllListeners();
            nodes.composition_btn.onClick.RemoveAllListeners();
            nodes.comp_close_btn.onClick.RemoveAllListeners();
            nodes.comp_add_btn.onClick.RemoveAllListeners();
            nodes.comp_edit_btn.onClick.RemoveAllListeners();
            nodes.comp_delete_btn.onClick.RemoveAllListeners();
            nodes.compConfirm_btn.onClick.RemoveAllListeners();
        }

        private void InitData()
        {
            nodes.division_dropdown.value = FumenEditorManager.Instance.CurrentDivisionIndex;
            editorViewportPosition = FumenEditorManager.Instance.CreateEditorViewportPosition(nodes.background_rawimage);
        }

        private void InitContent()
        {
            nodes.open_btn.onClick.AddListener(() => OpenFile());
            nodes.save_btn.onClick.AddListener(() => SaveFile());
            nodes.play_btn.onClick.AddListener(() => PlayMusic());
            nodes.pause_btn.onClick.AddListener(() => StopMusic());
            nodes.brick_btn.onClick.AddListener(() => AddFieldObject("Brick"));
            nodes.jump_btn.onClick.AddListener(() => AddFieldObject("Jump"));
            nodes.slide_btn.onClick.AddListener(() => AddFieldObject("Slide"));
            nodes.kick_btn.onClick.AddListener(() => AddFieldObject("Kick"));
            nodes.lantern_btn.onClick.AddListener(() => AddFieldObject("Lantern"));
            nodes.delete_btn.onClick.AddListener(() => DeleteFieldObject());
            nodes.bar_input.onEndEdit.AddListener((text) => ParseCurrentBar(text));
            nodes.division_dropdown.onValueChanged.AddListener((value) => ChangeDivision(value));
            nodes.barSlider_slider.onValueChanged.AddListener((value) => OnBarSliderValueChanged(value));
            nodes.preview_btn.onClick.AddListener(() => Preview());
            nodes.composition_btn.onClick.AddListener(() => ShowCompositionEditor());
            nodes.comp_close_btn.onClick.AddListener(() => CloseCompositionEditor());
            nodes.comp_add_btn.onClick.AddListener(() => AddComposition());
            nodes.comp_edit_btn.onClick.AddListener(() => EditComposition());
            nodes.comp_delete_btn.onClick.AddListener(() => DeleteComposition());
            nodes.compConfirm_btn.onClick.AddListener(() => CompositionEditConfirm());


            FumenEditorManager.Instance.RefreshStateAction += () =>
            {
                nodes.barSlider_slider.value = FumenEditorManager.Instance.CurrentBar;
                nodes.bar_input.text = FumenEditorManager.Instance.CurrentBar.ToString();
            };
        }

        #region UI View
        private void DrawLines()
        {
            int currentDivision = FumenEditorManager.Instance.CurrentDivision;
            float editorTop = editorViewportPosition[0];
            float editorLeft = editorViewportPosition[1];
            float editorBottom = editorViewportPosition[2];
            float editorRight = editorViewportPosition[3];

            float height = editorTop - editorBottom;
            //画拍线
            for (int i = 0; i < 5; i++)
            {
                Vector3 start = new(editorLeft, editorBottom + height / 4 * i);
                Vector3 end = new(editorRight, editorBottom + height / 4 * i);
                Color color = i > 0 && i < 4 ? Color.green : Color.red;
                LineController.Instance.AddLine(new DataStudio.Line(start, end, color));
            }

            //画细分线
            for (int i = 0; i < currentDivision; i++)
            {
                int skip = currentDivision == 16 ? 4 : 2;
                if (i % skip == 0) continue;
                Vector3 start = new(editorLeft, editorBottom + height / currentDivision * i);
                Vector3 end = new(editorRight, editorBottom + height / currentDivision * i);
                LineController.Instance.AddLine(new DataStudio.Line(start, end));
            }

            //画中线
            for (int i = 1; i <= 2; i++)
            {
                float middle = editorLeft + (editorRight - editorLeft) / 3.0f * i;
                Vector3 midStart = new(middle, editorBottom);
                Vector3 midEnd = new(middle, editorTop);
                LineController.Instance.AddLine(new DataStudio.Line(midStart, midEnd));
            }
        }

        #endregion

        #region Event

        private void OpenFile()
        {
            FumenEditorManager.Instance.OpenFile();
            nodes.barSlider_slider.maxValue = FumenEditorManager.Instance.TotalBar;
            DrawLines();
            ChangeDivision(1);
        }

        private void SaveFile()
        {
            FumenEditorManager.Instance.SaveFile();
        }

        private void PlayMusic()
        {
            FumenEditorManager.Instance.PlayMusic();
        }

        private void StopMusic()
        {
            FumenEditorManager.Instance.StopMusic();
            nodes.bar_input.text = FumenEditorManager.Instance.CurrentBar.ToString();
        }

        private void Preview()
        {
            LineController.Instance.ClearAllLines();
            HideSelf();
            UIManager.Instance.CreatePanel(PanelEnum.Test, new TestPanel.TestPanelOption
            {
                startTime = FumenEditorManager.Instance.GetPreviewTime()
            });
            FumenEditorManager.Instance.Preview();
        }

        private void AddFieldObject(string obj)
        {
            FumenEditorManager.Instance.AddFieldObject(obj);
        }

        private void DeleteFieldObject()
        {
            FumenEditorManager.Instance.DeleteFieldObject();

        }

        private void ParseCurrentBar(string s)
        {
            FumenEditorManager.Instance.CurrentBar = int.Parse(s);
        }

        private void ChangeDivision(int value)
        {
            FumenEditorManager.Instance.CurrentDivisionIndex = value;
            LineController.Instance.ClearAllLines();
            DrawLines();
        }

        private void OnBarSliderValueChanged(float value)
        {
            FumenEditorManager.Instance.CurrentBar = (int)value;
        }

        private void ShowCompositionEditor()
        {
            if (FumenEditorManager.Instance.IsLoaded)
            {
                LineController.Instance.ClearAllLines();
                FumenEditorManager.Instance.ClearItems();
                nodes.compositionWindow.SetActive(true);
                LoadCompositions();
            }
        }

        private void LoadCompositions()
        {
            compositionWidgetList ??= new();
            compositionWidgetList.ForEach((item) => Destroy(item));
            compositionWidgetList.Clear();

            var compositions = FumenEditorManager.Instance.GetCompositionList();
            foreach (var item in compositions)
            {
                var obj = Instantiate(compositionWidget);
                obj.transform.SetParent(nodes.compositionContent_list.transform);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<Widget.CompositionWidget>().SetData(item);
                compositionWidgetList.Add(obj);
            }
        }

        private void UnloadCompositions()
        {
            compositionWidgetList.ForEach((item) => Destroy(item));
            compositionWidgetList.Clear();
        }

        private void CloseCompositionEditor()
        {
            UnloadCompositions();
            nodes.compositionWindow.SetActive(false);
            DrawLines();
            FumenEditorManager.Instance.GenerateItems();
        }

        private void AddComposition()
        {
            nodes.edit_panel.SetActive(true);
        }

        private void EditComposition()
        {
            nodes.edit_panel.SetActive(true);
        }

        private void DeleteComposition()
        {

        }

        private void CompositionEditConfirm()
        {
            CompositionType type = (CompositionType)nodes.comp_dropdown.value;
            uint met = uint.Parse(nodes.met_input.text);
            uint submet = uint.Parse(nodes.submet_input.text);
            uint para = uint.Parse(nodes.para_input.text);
            Composition composition = null;
            switch (type)
            {
                case CompositionType.Fly:
                    composition = new FlyComposition(met, submet, para);
                    break;
                case CompositionType.Boss:
                    composition = new BossComposition(met, submet, para);
                    break;
                case CompositionType.Tutorial:
                    composition = new TutorialComposition(met, submet, para);
                    break;
                case CompositionType.Credit:
                    composition = new CreditComposition(met, submet, para);
                    break;
                default:
                    break;
            }
            if (composition != null)
            {
                FumenEditorManager.Instance.AddComposition(composition);
            }
            LoadCompositions();
            nodes.edit_panel.SetActive(false);

        }

        #endregion
    }

}
