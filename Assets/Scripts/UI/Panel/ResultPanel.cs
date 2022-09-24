using Runner.Core;
using Runner.GamePlay;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputAction;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 结算界面UI面板: ResultPanel
    /// Author: 单元琛 2022-8-24
    /// </summary>
    public class ResultPanel : BasePanel
    {
        private ResultPanel_Nodes nodes;
        private int currentCollection, totalCollection, score;

        protected override void OnStart()
        {
            nodes = rawNodes as ResultPanel_Nodes;
            InitData();
            InitContent();
        }

        private void InitData()
        {
            currentCollection = ObjectManager.Instance.CurrentCollection;
            totalCollection = ObjectManager.Instance.GetTotalCollections();
            score = GamePlayManager.Instance.Score;
            int id = GamePlayManager.MusicID;
            SaveManager.Instance.SaveLevelData(id, currentCollection, score);
        }

        private void InitContent()
        {
            UIManager.Instance.AddConfirmAction(ReturnMain);
            UIManager.Instance.AddCancelAction(RetryGame);

            nodes.retry_btn.onClick.AddListener(RetryGame);
            nodes.continue_btn.onClick.AddListener(ReturnMain);
            ShowResultInfo();
        }

        protected override void OnPanelDestroy()
        {
            UIManager.Instance.RemoveConfirmAction(ReturnMain);
            UIManager.Instance.RemoveCancelAction(RetryGame);

            nodes.retry_btn.onClick.RemoveListener(RetryGame);
            nodes.continue_btn.onClick.RemoveListener(ReturnMain);
        }

        private void ShowResultInfo()
        {
            nodes.collection_txt.text = $"{currentCollection} / {totalCollection}";
            nodes.score_txt.text = score.ToString();
            int count = GamePlayManager.Instance.fumenData.collections.Count;
            int totalScore = count * 5000;// todo 读表
            nodes.grade_txt.text = GetGrade(score, totalScore);
        }

        private string GetGrade(int score, int totalScore)
        {
            float percent = (float)score / totalScore;
            if (percent > 0.98f) return "S++";
            else if (percent > 0.95f) return "S+";
            else if(percent > 0.9f) return "S";
            else if(percent > 0.8f) return "A";
            else if(percent > 0.7f) return "B";
            else if(percent > 0.6f) return "C";
            else return "D";
        }

        private void RetryGame()
        {
            AudioManager.Instance.StopMusic(AudioEnum.BGM);
            GamePlayManager.Instance.ResumeGame();
            GamePlayManager.Instance.Retry();
            UIManager.Instance.ShowPanel(PanelEnum.Play);
            CloseSelf();
        }

        private void RetryGame(CallbackContext ctx) => RetryGame();

        private void ReturnMain()
        {
            AudioManager.Instance.StopMusic(AudioEnum.BGM);
            AudioManager.Instance.SetLabel("Select");
            GamePlayManager.Instance.ResumeGame();
            if (GamePlayManager.MusicID == 8)
            {
                UIManager.Instance.DestroyPanel(PanelEnum.Play);
                GamePlayManager.Instance.ShowCredits();
                UIManager.Instance.DisableUIAction();
                CloseSelf();
                return;  
            }

            GamePlayManager.Instance.ReturnMain();
            UIManager.Instance.DestroyPanel(PanelEnum.Play);
            CloseSelf();
        }

        private void ReturnMain(CallbackContext ctx) => ReturnMain();


    }

}
