using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runner.DataStudio.Serialize;

namespace Runner.UI.Widget
{
    public class CompositionWidget : BaseWidget
    {
        public TextMeshProUGUI type, met, submet, para;
        
        public void SetData(Composition composition)
        {
            type.text = composition.compositionType.ToString();
            met.text = composition.met.ToString();
            submet.text = composition.submet.ToString();
            para.text = composition.para.ToString();
        }
    }

}
