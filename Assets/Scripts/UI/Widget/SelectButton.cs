using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.UI.Widget
{
    /// <summary>
    /// 选择按钮: SelectButton
    /// Author: 单元琛 2022-8-30
    /// </summary>
    public class SelectButton : Button
    {
        public SelectWidget SelectLevelWidget { get; private set; }
        public void SetSelectLevelWidget(SelectWidget widget) => SelectLevelWidget = widget;
        protected override void OnEnable()
        {
            base.OnEnable();
            SetSelectLevelWidget(GetComponent<SelectWidget>());
        }
        public override Selectable FindSelectableOnDown()
        {
            var widget = base.FindSelectableOnDown() as SelectButton;
            if (widget == null) return this;
            SelectLevelWidget.SetSelected(false);
            widget.SelectLevelWidget.SetSelected(true);
            return widget;
        }

        public override Selectable FindSelectableOnUp()
        {
            var widget = base.FindSelectableOnUp() as SelectButton;
            if (widget == null) return this;
            SelectLevelWidget.SetSelected(false);
            widget.SelectLevelWidget.SetSelected(true);
            return widget;
        }

        public override Selectable FindSelectableOnLeft()
        {
            var widget = base.FindSelectableOnLeft() as SelectButton;
            if (widget == null) return this;
            SelectLevelWidget.SetSelected(false);
            widget.SelectLevelWidget.SetSelected(true);
            return widget;
        }

        public override Selectable FindSelectableOnRight()
        {
            var widget = base.FindSelectableOnRight() as SelectButton;
            if (widget == null) return this;
            SelectLevelWidget.SetSelected(false);
            widget.SelectLevelWidget.SetSelected(true);
            return widget;
        }
    }
}
