using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.UI.Widget
{
    public interface ISelectableWidget
    {
        public void SetSelected(bool selected);
    }

    /// <summary>
    /// 选择控件: SelectWidget
    /// Author: 单元琛 2022-8-30
    /// </summary>
    public class SelectWidget : BaseWidget, ISelectableWidget, IPointerEnterHandler, IPointerExitHandler
    {
        protected SelectButton button;

        public void AddListener(UnityAction call) => button.onClick.AddListener(call);

        public void RemoveButtonListener(UnityAction call) => button.onClick.RemoveListener(call);

        public void RemoveAllListeners() => button.onClick.RemoveAllListeners();

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
        }

        public virtual void SetSelected(bool selected)
        {
        }

        public void DisableSelection() => button.navigation = new() { mode = Navigation.Mode.None };

        public void EnableSelection() => button.navigation = Navigation.defaultNavigation;

    }
}
