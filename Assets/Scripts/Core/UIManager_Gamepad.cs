using UnityEngine;
using Runner.Utils;
using System;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

namespace Runner.Core
{
    /// <summary>
    /// UI管理器: UIManager
    /// Author: 单元琛 2022-8-9
    /// </summary>
    public partial class UIManager : Singleton<UIManager>
    {
        public void AddConfirmAction(Action<CallbackContext> context)
        {
            playerControls.UI.Confirm.performed += context;
        }

        public void RemoveConfirmAction(Action<CallbackContext> context)
        {
            playerControls.UI.Confirm.performed -= context;
        }

        public void AddAnyKeyAction(Action<CallbackContext> context)
        {
            playerControls.UI.AnyKey.performed += context;
        }

        public void RemoveAnyKeyAction(Action<CallbackContext> context)
        {
            playerControls.UI.AnyKey.performed -= context;
        }


        public void AddCancelAction(Action<CallbackContext> context)
        {
            playerControls.UI.Cancel.performed += context;
        }

        public void RemoveCancelAction(Action<CallbackContext> context)
        {
            playerControls.UI.Cancel.performed -= context;
        }

        public void AddMenuAction(Action<CallbackContext> context)
        {
            playerControls.UI.Menu.performed += context;
        }

        public void RemoveMenuAction(Action<CallbackContext> context)
        {
            playerControls.UI.Menu.performed -= context;
        }

        public void AddUpAction(Action<CallbackContext> context)
        {
            playerControls.UI.Up.performed += context;
        }

        public void RemoveUpAction(Action<CallbackContext> context)
        {
            playerControls.UI.Up.performed -= context;
        }

        public void AddDownAction(Action<CallbackContext> context)
        {
            playerControls.UI.Down.performed += context;
        }

        public void RemoveDownAction(Action<CallbackContext> context)
        {
            playerControls.UI.Down.performed -= context;
        }

        public void AddLeftAction(Action<CallbackContext> context)
        {
            playerControls.UI.Left.performed += context;
        }

        public void RemoveLeftAction(Action<CallbackContext> context)
        {
            playerControls.UI.Left.performed -= context;
        }

        public void AddRightAction(Action<CallbackContext> context)
        {
            playerControls.UI.Right.performed += context;
        }

        public void RemoveRightAction(Action<CallbackContext> context)
        {
            playerControls.UI.Right.performed -= context;
        }

        public void AddNextAction(Action<CallbackContext> context)
        {
            playerControls.UI.Next.performed += context;
        }

        public void RemoveNextAction(Action<CallbackContext> context)
        {
            playerControls.UI.Next.performed -= context;
        }

        public void EnableUIAction()
        {
            playerControls?.UI.Enable();
        }

        public void DisableUIAction()
        {
            playerControls?.UI.Disable();
        }

    }

}
