using Runner.Core;
using Runner.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Runner.Start;
using static UnityEngine.InputSystem.InputAction;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 设置UI面板: SettingPanel
    /// Author: 单元琛 2022-8-21
    /// </summary>
    public class SettingPanel : BasePanel
    {
        private SettingPanel_Nodes nodes;
        private float musicVolume, seVolume;
        private bool showMiddle;
        protected override void OnStart()
        {
            nodes = rawNodes as SettingPanel_Nodes;
            InitData();
            InitContent();
        }

        private void InitData()
        {
            var data = SaveManager.Instance.GetPlayerData();
            musicVolume = (float)data.musicVolume;
            seVolume = (float)data.seVolume;
            showMiddle = data.showMiddle;
        }

        private void InitContent()
        {
            StartManager.Instance.ChangeCameraBlurOn();
            UIManager.Instance.AddCancelAction(QuitSetting);
            gameObject.transform.localPosition = new Vector3(1920, 0);
            gameObject.transform.DOLocalMoveX(0, .75f);
            nodes.back_btn.onClick.AddListener(() => QuitSetting());
            nodes.reset_btn.onClick.AddListener(() => ResetSave());

            nodes.music_slider.value = musicVolume;
            nodes.se_slider.value = seVolume;
            nodes.model_slider.value = showMiddle ? 1 : 0;

            nodes.musicSliderValue_txt.text = $"{Mathf.CeilToInt(nodes.music_slider.value * 100.0f)}%";
            nodes.seSliderValue_txt.text = $"{Mathf.CeilToInt(nodes.se_slider.value * 100.0f)}%";
            nodes.modelSliderValue_txt.text = showMiddle ? "高" : "低";

            nodes.music_slider.onValueChanged.AddListener(value => ChangeMusicVolume(value));
            nodes.se_slider.onValueChanged.AddListener(value => ChangeSEVolume(value));
            nodes.model_slider.onValueChanged.AddListener(value => ChangeModelSlider(value));
        }

        protected override void OnPanelDestroy()
        {
            UIManager.Instance.RemoveCancelAction(QuitSetting);
            nodes.back_btn.onClick.RemoveAllListeners();
            nodes.reset_btn.onClick.RemoveAllListeners();
            nodes.music_slider.onValueChanged.RemoveAllListeners();
            nodes.se_slider.onValueChanged.RemoveAllListeners();
        }

        private void ChangeMusicVolume(float value)
        {
            nodes.musicSliderValue_txt.text = $"{Mathf.CeilToInt(value * 100.0f)}%";
            AudioManager.Instance.SetMusicVolume(value);
        }

        private void ChangeSEVolume(float value)
        {
            nodes.seSliderValue_txt.text = $"{Mathf.CeilToInt(value * 100.0f)}%";
        }

        private void ChangeModelSlider(float value)
        {
            if (value == 0)
            {
                nodes.modelSliderValue_txt.text = "低";
            }
            else
            {
                nodes.modelSliderValue_txt.text = "高";
            }
        }

        private void QuitSetting()
        {
            StartManager.Instance.ChangeCameraBlurOff();
            StartManager.Instance.ChangeCamera(PanelEnum.Main);
            SaveManager.Instance.SetVolume(nodes.music_slider.value, nodes.se_slider.value);
            SaveManager.Instance.SetShowMiddle(nodes.model_slider.value == 1);
            var seq = DOTween.Sequence().
                Append(gameObject.transform.DOLocalMoveX(1920, .5f)).
                AppendCallback(() => ReturnToMainPanel());
            seq.Play();
        }

        private void QuitSetting(CallbackContext ctx) => QuitSetting();

        private void ReturnToMainPanel()
        {
            AudioManager.Instance.SetLabel("Title");
            CloseSelf();
            UIManager.Instance.ShowPanel(PanelEnum.Main);
        }

        private void ResetSave()
        {
            GameManager.Instance.isFirstBoot = true;
            SaveManager.Instance.ResetData();
            SceneManager.UnloadSceneAsync(SceneEnum.Start.ToString());
            AudioManager.Instance.SetLabel("Title");
            GameManager.Instance.RestartGame();
        }

    }

}
