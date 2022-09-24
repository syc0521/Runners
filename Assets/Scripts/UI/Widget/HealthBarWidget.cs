using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI.Widget
{
    public class HealthBarWidget : BaseWidget
    {
        public Image healthImage;
        public void SetHealth(float health)
        {
            healthImage.fillAmount = health;
        }
    }

}
