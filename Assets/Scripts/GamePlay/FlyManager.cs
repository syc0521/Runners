using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Utils;
using DG.Tweening;

namespace Runner.GamePlay
{
    public class FlyManager : Singleton<FlyManager>
    {
        private Transform playerTrans;
        private Transform objectGenNode;
        private PlayerController playerController;

        public float height = 15f;
        public float tweenTime=2f;

        public Sequence flySequence { get; private set; }
        public Sequence backSequence { get; private set; }


        protected override void OnAwake()
        {
            flySequence = DOTween.Sequence();
            backSequence = DOTween.Sequence();

            base.OnAwake();
            playerTrans = GameObject.FindWithTag("Player")?.transform;

            objectGenNode = GameObject.Find("ObjectGenNode")?.transform;

            if (playerTrans == null || objectGenNode == null) return;

            playerController = playerTrans.gameObject.GetComponent<PlayerController>();
            var playerOriginPosY=playerTrans.position.y;
            var objectGenNodeOrginPosY=objectGenNode.position.y;
            
            flySequence.Join(playerTrans.DOMoveY(playerOriginPosY + height, tweenTime).SetAutoKill(false).Pause().SetEase(Ease.Linear)).SetAutoKill(false).Pause().onComplete += () =>
            {
                playerController.OnFlyToTopTweenComplete();
            };

            backSequence.Join(playerTrans.DOMoveY(playerOriginPosY, tweenTime).SetAutoKill(false).Pause().SetEase(Ease.Linear)).SetAutoKill(false).Pause().onComplete += () =>
            {
                playerController.OnBackToBottomTweenComplete();
            };      
            Debug.Log("FlyManagerAwake");
        }

        [ContextMenu("Fly")]
        public void FlyToTop()
        {           
            Debug.LogWarning("FlyToTop");
            flySequence.Restart();
            playerController.OnFlyToTopTween();
        }

        [ContextMenu("Back")]
        public void BackToBottom()
        {
            backSequence.Restart();
            playerController.OnBackToBottomTween();
        }

        public void OnDisable()
        {
            Debug.Log("FlyManagerDisable");
        }
    }
}


