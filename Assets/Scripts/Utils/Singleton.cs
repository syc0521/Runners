using System;
using UnityEngine;

namespace Runner.Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>, new()
    {
        private static T _instance;
        public static T Instance { get { return _instance; } }
        protected virtual void Awake()
        {
            _instance = (T)this;
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }

}
