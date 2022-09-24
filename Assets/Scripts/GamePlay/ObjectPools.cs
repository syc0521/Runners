using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.GamePlay
{
    public class BaseFieldObjectPool : Utils.CustomObjectPool
    {
        public BaseFieldObjectPool(GameObject obj) : base(obj) { }
        protected override void OnCreate(ref GameObject gameObject)
        {
            
        }

        protected override void OnGet(GameObject gameObject)
        {
        }

        protected override void OnRelease(GameObject gameObject)
        {
        }
    }


    public class CollectionPool : BaseFieldObjectPool
    {
        public CollectionPool(GameObject obj) : base(obj) { }
    }

    public class ObstaclePool : BaseFieldObjectPool
    {
        public ObstaclePool(GameObject obj) : base(obj) { }
    }
}
