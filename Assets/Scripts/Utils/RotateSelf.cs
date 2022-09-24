using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Utils
{
    public class RotateSelf : MonoBehaviour
    {
        public float speed=30f;
        
        void Update()
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime,Space.World);
        }
    }

}
