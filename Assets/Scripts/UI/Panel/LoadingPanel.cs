using DG.Tweening;
using Runner.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.UI.Panel
{
    /// <summary>
    /// 加载UI面板: LoadingPanel
    /// Author: 单元琛 2022-8-22
    /// </summary>
    public class LoadingPanel : BasePanel
    {
        public class Option : PanelOption
        {
            public SceneEnum sceneEnum;
        }
        private LoadingPanel_Nodes nodes;

        protected override void OnStart()
        {
            nodes = rawNodes as LoadingPanel_Nodes;            
            InitContent();
        }

        private void InitContent()
        {
            StartCoroutine(LoadScene((option as Option).sceneEnum.ToString()));
        }

        private IEnumerator LoadScene(string name)
        {
            yield return new WaitForSeconds(0.25f);
            nodes.loading_txt.gameObject.SetActive(true);

            DOTween.To(() => 0.0f, x => nodes.black_img.color = new Color(0, 0, 0, x), 1f, 1.5f).Play();
            DOTween.To(() => 0.0f, x => nodes.loadingBack_img.color = new Color(1, 1, 1, x), 1f, 1.5f).Play();
            DOTween.To(() => 0.0f, x => nodes.round_img.color = new Color(1, 1, 1, x), 1f, 1.5f).Play();

            yield return new WaitForSeconds(0.25f);
            DOTween.To(() => 0.0f, x => nodes.leftLine_img.fillAmount = x, 1f, 1.1f).Play();
            DOTween.To(() => 0.0f, x => nodes.rightLine_img.fillAmount = x, 1f, 1.1f).Play();
            DOTween.To(() => 0.0f, x => nodes.middleLine_img.fillAmount = x, 1f, 1.1f).Play();

            var task = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            task.allowSceneActivation = false;
            while (task.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1.4f);
            task.allowSceneActivation = true;
            CloseSelf();
        }
    }
}
