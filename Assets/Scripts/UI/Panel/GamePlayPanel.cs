using Runner.Core;
using Runner.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 游戏界面UI面板: GamePlayPanel
    /// Author: 单元琛 2022-8-11
    /// </summary>
    public class GamePlayPanel : BasePanel
    {
        private GamePlayPanel_Nodes nodes;
        private int totalCollection;
        protected override void OnStart()
        {
            nodes = rawNodes as GamePlayPanel_Nodes;
            InitContent();
            ObjectManager.Instance.CollectionHandler += RefreshPanel;
            totalCollection = ObjectManager.Instance.GetTotalCollections();
            LoopSceneManager.Instance.Play();
            RefreshPanel();
        }

        protected override void OnShown()
        {
            UIManager.Instance.AddMenuAction(PauseGame);
            RefreshPanel();
        }

        protected override void OnHidden()
        {
            UIManager.Instance.RemoveMenuAction(PauseGame);
            RefreshPanel();
        }

        private void InitContent()
        {
            UIManager.Instance.AddMenuAction(PauseGame);
            nodes.pause_btn.onClick.AddListener(PauseGame);
            GamePlayManager.Instance.playerHealth.OnValueChangedEvent += (value) =>
            {
                nodes.healthbar_w?.SetHealth(value / 100.0f);
            };
        }

        protected override void OnPanelDestroy()
        {
            UIManager.Instance.RemoveMenuAction(ctx => PauseGame());
            nodes.pause_btn.onClick.RemoveListener(PauseGame);
        }

        private void PauseGame()
        {
            HideSelf();
            GamePlayManager.Instance.PauseGame();
        }

        private void PauseGame(CallbackContext ctx) => PauseGame();

        private void RefreshPanel()
        {
            nodes.score_txt.text = $"{ObjectManager.Instance.CurrentCollection} / {totalCollection}";
        }

    }

}
