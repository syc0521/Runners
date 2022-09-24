using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Utils
{
    public class FloatSelf : MonoBehaviour
    {
        [Header("¸¡¶¯Õñ·ù")]
        public float amplitude = 0.1f;

        [Header("¸¡¶¯ÆµÂÊ")]
        public float frequency = 1.2f;


        private Vector3 originPos;

        private Vector3 endPos;

        void Start()
        {
            originPos = transform.localPosition;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            endPos = originPos;
            endPos.y=Mathf.Sin(Time.fixedTime*Mathf.PI*frequency)*amplitude+originPos.y;
            transform.localPosition = endPos;
        }
    }

}

