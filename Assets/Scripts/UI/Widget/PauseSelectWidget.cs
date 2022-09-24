using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.UI.Widget
{
    /// <summary>
    /// 暂停界面控件: PauseSelectWidget
    /// Author: 单元琛 2022-9-2
    /// </summary>
    public class PauseSelectWidget : SelectWidget
    {
        public Image selected_img;

        public void Initialize(bool selected = false)
        {
            button = GetComponent<SelectButton>();
            button.SetSelectLevelWidget(this);
            SetSelected(selected);
        }

        public override void SetSelected(bool b) => selected_img.gameObject.SetActive(b);

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
            SelectThis();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UnSelectThis();
        }

    }

}
