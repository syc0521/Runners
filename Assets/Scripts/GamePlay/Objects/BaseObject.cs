using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.GamePlay.Objects
{
    public interface IMoveableObject
    {
        void MoveObject();
    }
    public interface ICollisionableObject
    {
        void OnCollision();
    }

    public abstract class BaseObject : MonoBehaviour
    {
        public DataStudio.Serialize.BaseObject data;
    }

}
