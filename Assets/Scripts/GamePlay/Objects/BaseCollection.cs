using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.GamePlay.Objects
{
    public class BaseCollection : BaseFieldObject
    {
        public override void PlaySound()
        {
            ObjectManager.Instance.PlayCollectionSound();
        }
    }
}

