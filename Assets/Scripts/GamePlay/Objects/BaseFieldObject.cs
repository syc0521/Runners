using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Runner.GamePlay.Objects
{
    public class BaseFieldObject : BaseObject, IMoveableObject
    {
        public int damageValue, scoreValue;
        public Vector3 startPos, endPos;
        protected float moveTime;
        protected void Awake()
        {
            moveTime = ObjectManager.baseMovetime;
        }
        protected void Start()
        {

        }
        void Update()
        {
            MoveObject();
        }

        public void MoveObject()
        {
            moveTime = ObjectManager.baseMovetime;
            var currentTime = GamePlayManager.Instance.CurrentTime;
            if (currentTime < data.time + moveTime + 0.5f)
            {
                var endTime = data.time + moveTime;
                var fixedEndPos = endPos.x + 0.1f;
                var currentPos = new Vector3(Utils.Utils.Lerp(currentTime + moveTime, data.time, endTime, startPos.x, fixedEndPos), transform.position.y, transform.position.z);
                transform.position = currentPos;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public virtual void PlaySound()
        {

        }

    }
}