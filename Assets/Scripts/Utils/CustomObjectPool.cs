using UnityEngine;
using UnityEngine.Pool;

namespace Runner.Utils
{
    public class CustomObjectPool
    {
        protected ObjectPool<GameObject> pool;
        public GameObject obj;

        public CustomObjectPool(GameObject obj)
        {
            this.obj = obj;
            pool = new ObjectPool<GameObject>(Create, Get, Release, Destroy, true, 20, 100);
        }

        private GameObject Create()
        {
            var item = Object.Instantiate(obj);
            OnCreate(ref item);
            return item;
        }
        private void Get(GameObject gameObject)
        {
            OnGet(gameObject);
        }
        private void Release(GameObject gameObject)
        {
            OnRelease(gameObject);
        }
        private void Destroy(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        public GameObject GetObject() { return pool.Get(); }
        public void ReleaseObject(GameObject obj) { pool.Release(obj); }
        protected virtual void OnCreate(ref GameObject gameObject) { }
        protected virtual void OnGet(GameObject gameObject) { }
        protected virtual void OnRelease(GameObject gameObject) { }

    }
}
