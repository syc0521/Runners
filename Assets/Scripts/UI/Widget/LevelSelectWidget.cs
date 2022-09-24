using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.UI.Widget
{
    /// <summary>
    /// 选关界面关卡控件: LevelSelectWidget
    /// Author: 单元琛 2022-8-29
    /// </summary>
    public class LevelSelectWidget : SelectWidget
    {
        public Image selected_img;
        public TMPro.TextMeshProUGUI level_text;
        public bool IsUnlock { get; private set; }

        public void Initialize(string text, bool unlocked = false, bool selected = false)
        {
            button = GetComponent<SelectButton>();
            button.SetSelectLevelWidget(this);
            SetText(text);
            SetUnlocked(unlocked);
            SetSelected(selected);
        }

        public void SetText(string text) => level_text.text = text;


        public override void SetSelected(bool b) => selected_img.gameObject.SetActive(b);

        public void SetUnlocked(bool b)
        {
            IsUnlock = b;
            button.interactable = b;
        }

        public SelectButton GetSelectButton() => button;

        public void SelectThis()
        {
            EventSystem.current.SetSelectedGameObject(gameObject, new(EventSystem.current));
            button.OnSelect(new(EventSystem.current));
            SetSelected(true);
        }
        public void UnSelectThis()
        {
            SetSelected(false);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (IsUnlock)
            {
                SelectThis();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UnSelectThis();
        }
    }

}
