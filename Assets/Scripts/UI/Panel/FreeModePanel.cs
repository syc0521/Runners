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
    public class FreeModePanel : BasePanel
    {
        private FreeModePanel_Nodes nodes;
        private List<MusicTableData> songList;
        private Dictionary<int, LevelData> levelDatas;
        private int currentID;
        private LevelSelectWidget currentSelected;

        private static int normalSongNum = 5;
        protected override void OnStart()
        {
            nodes = rawNodes as FreeModePanel_Nodes;
            InitData();
            InitContent();
            StartManager.Instance.ChangeSelectSound("Freemode");
        }

        private void InitData()
        {
            songList = TableManager.Instance.GetSongList();
            levelDatas = SaveManager.Instance.GetUserLevelDatas();
        }

        protected override void OnShown()
        {
            currentSelected?.SelectThis();
            UIManager.Instance.AddCancelAction(ToNormalMode);

            AudioManager.Instance.SetLabel("Select");
            StartManager.Instance.ChangeCamera(PanelEnum.Select);
            nodes.confirmArea.transform.localPosition = new(650, 0);
            nodes.background_btn.gameObject.SetActive(false);

            SetUnlockSongs();
        }

        protected override void OnHidden()
        {
            UIManager.Instance.RemoveCancelAction(ToNormalMode);
            base.OnHidden();
        }

        private void InitContent()
        {
            gameObject.transform.localPosition = new Vector3(0, -1080);
            gameObject.transform.DOLocalMoveY(0, .5f);
            UIManager.Instance.AddCancelAction(ToNormalMode);
            nodes.back_btn.onClick.AddListener(ToNormalMode);
            nodes.start_btn.onClick.AddListener(StartGame);
            nodes.background_btn.onClick.AddListener(HideConfirmArea);

            //初始化按钮信息
            foreach (var item in nodes.levelButtons_w)
            {
                item.Initialize(TableManager.Instance.GetText(songList[nodes.levelButtons_w.IndexOf(item)+ normalSongNum].Nameid));
            }

            //设置解锁信息
            SetUnlockSongs();

            //添加绑定
            foreach (var item in nodes.levelButtons_w)
            {
                item.AddListener(() => ShowConfirmArea(songList[nodes.levelButtons_w.IndexOf(item)+ normalSongNum]));
            }

            nodes.levelButtons_w[0].SelectThis();
        }

        protected override void OnPanelDestroy()
        {
            UIManager.Instance.RemoveCancelAction(ToNormalMode);
            StartManager.Instance.ChangeSelectSound("MainTrack");
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
            UIManager.Instance.RemoveCancelAction(ToNormalMode);
            UIManager.Instance.AddConfirmAction(StartGame);
            UIManager.Instance.AddCancelAction(HideConfirmArea);
            nodes.confirmArea.transform.DOLocalMoveX(0, .5f);
            nodes.background_btn.gameObject.SetActive(true);
            nodes.songTitle_txt.text = TableManager.Instance.GetText(info.Nameid);
            nodes.songDesc_txt.text = TableManager.Instance.GetText(info.Descid);
            currentID = info.Musicid;
            nodes.collection_txt.text = levelDatas[info.Musicid].collection.ToString();
            nodes.score_txt.text = levelDatas[info.Musicid].score.ToString();
            StartManager.Instance.ChangeDecideSound("Decide");
        }

        private void HideConfirmArea()
        {
            currentSelected.SelectThis();
            nodes.levelButtons_w.ForEach((item) => item.EnableSelection());
            StartManager.Instance.ChangeCameraBlurOff();
            AudioManager.Instance.SetLabel("Select");
            UIManager.Instance.AddCancelAction(ToNormalMode);
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


        private void ToNormalMode()
        {
            var seq = DOTween.Sequence().
                Append(gameObject.transform.DOLocalMoveY(-1080, .5f));
            seq.Play().AppendCallback(()=>
            {
                UIManager.Instance.ShowPanel(PanelEnum.Select);
                CloseSelf();
            }); 
        }

        private void ToNormalMode(CallbackContext ctx) => ToNormalMode();

        private void ReturnToMainPanel()
        {
            AudioManager.Instance.SetLabel("Title");
            CloseSelf();
            UIManager.Instance.ShowPanel(PanelEnum.Main);
        }

        private void SetUnlockSongs()
        {          
            nodes.levelButtons_w.ForEach((v)=>
            {
                v.SetUnlocked(true);
            });
        }
    }
}
