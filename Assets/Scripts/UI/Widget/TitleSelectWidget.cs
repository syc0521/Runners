using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Runner.UI.Widget
{
    /// <summary>
    /// 主界面控件: TitleSelectWidget
    /// Author: 单元琛 2022-8-30
    /// </summary>
    public class TitleSelectWidget : SelectWidget, IWithBeatWidget
    {
        public Image selected_img;
        private Sequence seq;

        public void Initialize(bool selected = false)
        {
            button = GetComponent<SelectButton>();
            button.SetSelectLevelWidget(this);
            SetSelected(selected);
            float duration = 60.0f / 145.0f / 2.0f;
            selected_img.gameObject.transform.localScale = new(1.2f, 1.2f);
            seq = DOTween.Sequence().Append(
                selected_img.gameObject.transform.DOScale(0.9f, duration * 5.0f / 3.0f))
                .Append(selected_img.gameObject.transform.DOScale(1.2f, duration / 3.0f)).SetLoops(-1);
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

        public void PlayUIAnimation()
        {
            seq.Play();
        }
    }

}
