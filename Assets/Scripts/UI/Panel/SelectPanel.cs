using DG.Tweening;
using Runner.Core;
using Runner.DataStudio.Serialize;
using Runner.GamePlay;
using Runner.Start;
using Runner.UI.Widget;
using Runner.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 选关界面UI面板: SelectPanel
    /// Author: 单元琛 2022-8-21
    /// </summary>
    public class SelectPanel : BasePanel
    {
        private SelectPanel_Nodes nodes;
        private List<MusicTableData> songList;
        private Dictionary<int, LevelData> levelDatas;
        private int currentID;
        private LevelSelectWidget currentSelected;
        private const int bossID = 8, smallBossID = 5;
        protected override void OnStart()
        {
            nodes = rawNodes as SelectPanel_Nodes;
            InitData();
            InitContent();
        }

        private void InitData()
        {
            songList = TableManager.Instance.GetSongList();
            levelDatas = SaveManager.Instance.GetUserLevelDatas();
        }

        protected override void OnShown()
        {
            currentSelected?.SelectThis();
            UIManager.Instance.AddCancelAction(QuitSelect);
            UIManager.Instance.AddNextAction(ToFreeMode);
            AudioManager.Instance.SetLabel("Select");
            StartManager.Instance.ChangeCamera(PanelEnum.Select);
            nodes.confirmArea.transform.localPosition = new(650, 0);
            nodes.background_btn.gameObject.SetActive(false);

            SetUnlockSongs();
        }

        protected override void OnHidden()
        {
            UIManager.Instance.RemoveCancelAction(QuitSelect);
            UIManager.Instance.RemoveNextAction(ToFreeMode);
            base.OnHidden();
        }

        private void InitContent()
        {
            gameObject.transform.localPosition = new Vector3(0, -1080);
            gameObject.transform.DOLocalMoveY(0, .5f);
            UIManager.Instance.AddCancelAction(QuitSelect);
            nodes.back_btn.onClick.AddListener(QuitSelect);

            UIManager.Instance.AddNextAction(ToFreeMode);
            nodes.freeMode_btn.onClick.AddListener(ToFreeMode);

            nodes.start_btn.onClick.AddListener(StartGame);
            nodes.background_btn.onClick.AddListener(HideConfirmArea);

            //初始化按钮信息
            foreach (var item in nodes.levelButtons_w)
            {
                item.Initialize(TableManager.Instance.GetText(songList[nodes.levelButtons_w.IndexOf(item)].Nameid));
            }

            //设置解锁信息
            SetUnlockSongs();

            //添加绑定
            foreach (var item in nodes.levelButtons_w)
            {
                item.AddListener(() => ShowConfirmArea(songList[nodes.levelButtons_w.IndexOf(item)]));
            }

            nodes.levelButtons_w[0].SelectThis();
        }

        protected override void OnPanelDestroy()
        {
            UIManager.Instance.RemoveCancelAction(QuitSelect);
            UIManager.Instance.RemoveNextAction(ToFreeMode);
            nodes.back_btn.onClick.RemoveAllListeners();
            foreach (var item in nodes.levelButtons_w)
            {
                item.RemoveAllListeners();
            }
        }

        private void ShowConfirmArea(MusicTableData info)
        {
            currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<LevelSelectWidget>();
            nodes.levelButtons_w.ForEach((item) => item.DisableSelection());
            StartManager.Instance.ChangeCameraBlurOn();
            AudioManager.Instance.SetLabel("Select2");
            UIManager.Instance.RemoveNextAction(ToFreeMode);
            UIManager.Instance.RemoveCancelAction(QuitSelect);
            UIManager.Instance.AddConfirmAction(StartGame);
            UIManager.Instance.AddCancelAction(HideConfirmArea);
            nodes.confirmArea.transform.DOLocalMoveX(0, .5f);
            nodes.background_btn.gameObject.SetActive(true);
            nodes.songTitle_txt.text = TableManager.Instance.GetText(info.Nameid);
            nodes.songDesc_txt.text = TableManager.Instance.GetText(info.Descid);
            currentID = info.Musicid;
            nodes.collection_txt.text = levelDatas[info.Musicid].collection.ToString();
            nodes.score_txt.text = levelDatas[info.Musicid].score.ToString();
            if (info.Musicid == bossID)
            {
                StartManager.Instance.ChangeDecideSound("BossDecide");
            }
            else
            {
                StartManager.Instance.ChangeDecideSound("Decide");
            }
        }

        private void HideConfirmArea()
        {
            currentSelected.SelectThis();
            nodes.levelButtons_w.ForEach((item) => item.EnableSelection());
            StartManager.Instance.ChangeCameraBlurOff();
            AudioManager.Instance.SetLabel("Select");
            UIManager.Instance.AddCancelAction(QuitSelect);
            UIManager.Instance.AddNextAction(ToFreeMode);
            UIManager.Instance.RemoveConfirmAction(StartGame);
            UIManager.Instance.RemoveCancelAction(HideConfirmArea);
            nodes.confirmArea.transform.DOLocalMoveX(650, .5f);
            nodes.background_btn.gameObject.SetActive(false);
        }

        private void HideConfirmArea(CallbackContext ctx) => HideConfirmArea();

        private void StartGame()
        {
            nodes.levelButtons_w.ForEach((item) => item.EnableSelection());
            UIManager.Instance.RemoveConfirmAction(StartGame);
            UIManager.Instance.RemoveCancelAction(HideConfirmArea);
            GamepadVibrateManager.Instance.Vibrate(VibrateEnum.ExtraLongMedium);
            StartManager.Instance.StartGame(currentID, 0);
            UIManager.Instance.HidePanel(PanelEnum.Main);
            HideSelf();
        }

        private void StartGame(CallbackContext ctx) => StartGame();

        private void QuitSelect()
        {
            UIManager.Instance.RemoveCancelAction(QuitSelect);
            StartManager.Instance.ChangeCamera(PanelEnum.Main);
            var seq = DOTween.Sequence().
                Append(gameObject.transform.DOLocalMoveY(-1080, .5f)).
                AppendCallback(ReturnToMainPanel);
            seq.Play();
        }

        private void QuitSelect(CallbackContext ctx) => QuitSelect();

        private void ToFreeMode()
        {
            UIManager.Instance.DestroyPanel(PanelEnum.Freemode);
            UIManager.Instance.RemoveNextAction(ToFreeMode);
            UIManager.Instance.CreatePanel(PanelEnum.Freemode);
            HideSelf();
        }

        private void ToFreeMode(CallbackContext ctx) => ToFreeMode();

        private void ReturnToMainPanel()
        {
            AudioManager.Instance.SetLabel("Title");
            CloseSelf();
            UIManager.Instance.ShowPanel(PanelEnum.Main);
        }

        private void SetUnlockSongs()
        {
            nodes.levelButtons_w[0].SetUnlocked(true);
            for (int i = 1; i < nodes.levelButtons_w.Count; i++)
            {
                bool isUnlock = true;
                foreach (var id in songList[i].Unlockcondition)
                {
                    if (!levelDatas[id].isPassed)
                    {
                        isUnlock = false;
                        break;
                    }
                }
                nodes.levelButtons_w[i].SetUnlocked(isUnlock);
            }
            if (levelDatas[smallBossID].isPassed && !levelDatas[bossID].isPassed)
            {
                StartManager.Instance.ChangeSelectSound("Boss");
                for (int i = 0; i < nodes.levelButtons_w.Count - 1; i++)
                {
                    nodes.levelButtons_w[i].gameObject.SetActive(false);
                    nodes.levelButtons_w[i].UnSelectThis();
                }
                nodes.back_btn.gameObject.SetActive(false);
                nodes.levelButtons_w[4].SelectThis();
            }
            if (!levelDatas[smallBossID].isPassed)
            {
                nodes.levelButtons_w[4].gameObject.SetActive(false);
                //UIManager.Instance.RemoveCancelAction(QuitSelect);
            }
            else
            {
                nodes.levelButtons_w[4].gameObject.SetActive(true);
            }
        }
    }
}
