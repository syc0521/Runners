using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Utils;
using UnityEngine.UI;
using DG.Tweening;


namespace Runner.UI.Panel
{
    /// <summary>
    /// 转场面板节点: TransitionPanel
    /// Author: 路金磊 2022-8-28
    /// </summary>
    public class TransitionPanel : BasePanel
    {
        public class Option : PanelOption
        {
            public Camera grabCamera;
            public bool hasOpen = true;
            public bool hasClose = true;
        }

        private TransitionPanel_Nodes nodes;
        private Option opt;
        public float tweenTime = 0.5f;
        protected override void OnStart()
        {
            nodes = rawNodes as TransitionPanel_Nodes;
            opt = option as Option;
            InitContent();
            StartCoroutine(StartTransition());
        }

        private void InitContent()
        {
            nodes.transitionImgOuter.gameObject.SetActive(false);
            nodes.transitionImgInner.gameObject.SetActive(false);
        }

        private IEnumerator StartTransition()
        {
            if (opt.hasClose)
            {
                Close();
                yield return new WaitForSeconds(1.25f);
            }
            if (opt.hasOpen)
            {
                Open();
                yield return new WaitForSeconds(tweenTime);
            }
            CloseSelf();
        }

        [ContextMenu("Close")]
        public void Close()
        {
            SetGrabCamera();
            var mat = nodes.transitionImgOuter.material;
            var tex = RenderTexToTexture2D(opt.grabCamera.targetTexture);
            mat.SetTexture("_MainTex", tex);
            mat.SetFloat("_Power", 2);
            nodes.transitionImgOuter.gameObject.SetActive(true);
            RGBTween(mat, new Vector2(0.01f, 0), new Vector2(0.03f, 0f), new Vector3(0, 0.01f));
            mat.DOFloat(-2, "_Power", tweenTime);
        }


        [ContextMenu("Open")]
        public void Open()
        {
            SetGrabCamera();
            var mat = nodes.transitionImgInner.material;
            var tex = RenderTexToTexture2D(opt.grabCamera.targetTexture);
            mat.SetTexture("_MainTex", tex);
            mat.SetFloat("_Power", -20);
            nodes.transitionImgInner.gameObject.SetActive(true);
            nodes.transitionImgOuter.material.DOFloat(-10, "_Power", tweenTime);
            RGBTween(mat, new Vector2(0.01f, 0), new Vector2(0.03f, 0f), new Vector3(0, 0.01f), true);
            nodes.transitionImgInner.material.DOFloat(0, "_Power", tweenTime).onComplete += () =>
            {
                nodes.transitionImgInner.gameObject.SetActive(false);
                nodes.transitionImgOuter.gameObject.SetActive(false);
            };
        }

        private Texture2D RenderTexToTexture2D(RenderTexture rt)
        {
            int width = rt.width;
            int height = rt.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA64, false);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            return tex;
        }

        private void SetGrabCamera()
        {
            opt.grabCamera.gameObject.transform.position = Camera.main.transform.position;
            opt.grabCamera.gameObject.transform.rotation = Camera.main.transform.rotation;
        }

        private void RGBTween(Material mat, Vector2 rOffset, Vector2 gOffset, Vector2 bOffset, bool back = false)
        {
            if (!back)
            {
                mat.SetFloat("_R_OffsetX", 0);
                mat.SetFloat("_R_OffsetY", 0);
                mat.SetFloat("_G_OffsetX", 0);
                mat.SetFloat("_G_OffsetY", 0);
                mat.SetFloat("_B_OffsetX", 0);
                mat.SetFloat("_B_OffsetY", 0);
                mat.DOFloat(rOffset.x, "_R_OffsetX", tweenTime);
                mat.DOFloat(rOffset.y, "_R_OffsetY", tweenTime);
                mat.DOFloat(gOffset.x, "_G_OffsetX", tweenTime);
                mat.DOFloat(gOffset.y, "_G_OffsetY", tweenTime);
                mat.DOFloat(bOffset.x, "_B_OffsetX", tweenTime);
                mat.DOFloat(bOffset.y, "_B_OffsetY", tweenTime);

            }
            else
            {
                mat.SetFloat("_R_OffsetX", rOffset.x);
                mat.SetFloat("_R_OffsetY", rOffset.y);
                mat.SetFloat("_G_OffsetX", gOffset.x);
                mat.SetFloat("_G_OffsetY", gOffset.y);
                mat.SetFloat("_B_OffsetX", bOffset.x);
                mat.SetFloat("_B_OffsetY", bOffset.y);
                mat.DOFloat(0, "_R_OffsetX", tweenTime);
                mat.DOFloat(0, "_R_OffsetY", tweenTime);
                mat.DOFloat(0, "_G_OffsetX", tweenTime);
                mat.DOFloat(0, "_G_OffsetY", tweenTime);
                mat.DOFloat(0, "_B_OffsetX", tweenTime);
                mat.DOFloat(0, "_B_OffsetY", tweenTime);
            }
        }
    }
}

