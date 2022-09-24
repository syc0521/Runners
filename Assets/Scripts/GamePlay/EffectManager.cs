using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.DataStudio.Asset;
using Runner.Utils;

namespace Runner.GamePlay
{
    public class EffectManager:Singleton<EffectManager>
    {
        public EffectAsset effectTable;

        /// <summary>
        /// 播放一次特效
        /// </summary>
        /// <param name="name">特效名称</param>
        /// <param name="pos">出现位置</param>
        /// <returns>特效对象</returns>
        public GameObject PlayOneShot(EffectEnum name, Vector3 pos)
        {
            return PlayOneShot(name.ToString(), pos);
        }

        /// <summary>
        /// 播放一次特效
        /// </summary>
        /// <param name="name">特效名称</param>
        /// <param name="pos">出现位置</param>
        /// <returns>特效对象</returns>
        public GameObject PlayOneShot(string name,Vector3 pos)
        {
            if(!effectTable.effectDir.ContainsKey(name))
            {
                return null;
            }
            var tempEffect = effectTable.effectDir[name].gameObject;

            GameObject effect = Instantiate(tempEffect, pos, tempEffect.transform.rotation,GameObject.Find("Particles").transform);
            return effect;
        }
    }
}
