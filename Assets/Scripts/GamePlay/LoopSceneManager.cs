using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Utils;

namespace Runner.GamePlay
{
    public class LoopSceneManager : Singleton<LoopSceneManager>
    {
        public float middileSceneSpeed=1f;

        private LoopSceneController[] allLoopSceneControllers;
        private Dictionary<string, LoopSceneController> loopSceneColDic;

        protected override void OnAwake()
        {
            loopSceneColDic = new Dictionary<string, LoopSceneController>();
            allLoopSceneControllers = GameObject.FindObjectsOfType<LoopSceneController>();
            foreach(var v in allLoopSceneControllers)
            {
                loopSceneColDic.Add(v.gameObject.name, v);
            }
            if(loopSceneColDic.ContainsKey("CloserScenes"))
                loopSceneColDic["CloserScenes"].moveSpeed = ObjectManager.baseMoveSpeed;
            Debug.Log("LoopSceneMangerAwake");
        }

        public void Pause(string name="")
        {
            foreach (var v in loopSceneColDic.Values)
                v.pause = true;
        }

        public void Play(string name="")
        {
            foreach(var v in loopSceneColDic.Values)
            {
                v.pause=v.pause?false :v.pause;
            }
        }        
    }
}


