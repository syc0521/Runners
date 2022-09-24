using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Runner.UI.Widget
{
    public class TextWithBeat : TextMeshProUGUI, IWithBeatWidget
    {
        private Sequence seq;

        protected override void Start()
        {
            base.Start();
            float duration = 60.0f / 145.0f;
            color = new(1, 1, 1, 0);
            seq = DOTween.Sequence().AppendCallback(() => color = new(1, 1, 1, 1)).AppendInterval(duration).
                AppendCallback(() => color = new(1, 1, 1, 0)).AppendInterval(duration).SetLoops(-1);
            seq.Pause();
        }
        public void PlayUIAnimation()
        {
            seq.Play();
        }
    }

}
