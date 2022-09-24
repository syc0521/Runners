using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runner.DataStudio.Asset
{
    [CreateAssetMenu(fileName = "EffectTable", menuName = "ScriptableObjs/EffectTable")]
    public class EffectAsset : ScriptableObject
    {
        public Dictionary<string, ParticleSystem> effectDir = new Dictionary<string, ParticleSystem>();

        [System.Serializable]
        public struct Effect
        {
            public string name;
            public ParticleSystem particleSystem;
        }

        public Effect[] effects;

        private void OnEnable()
        {
            foreach (var v in effects)
            {
                if (!effectDir.ContainsKey(v.name))
                {
                    effectDir.Add(v.name, v.particleSystem);
                }
            }
        }
    }
}


