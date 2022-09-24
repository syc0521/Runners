using UnityEngine;

namespace Runner.UI.Widget
{
    public abstract class BaseWidget : MonoBehaviour
    {
        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnStart() { }

        protected virtual void OnUpdate() { }

    }

}
