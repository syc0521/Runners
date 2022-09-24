using Runner.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.GamePlay.Objects
{
    public class Brick : BaseCollection, ICollisionableObject
    {
        public void OnCollision()
        {
            GamepadVibrateManager.Instance.Vibrate(VibrateEnum.ShortLight);
            if (ObjectManager.Instance.CurrentCollection + 1 <= GamePlayManager.Instance.fumenData.collections.Count)
            {
                ObjectManager.Instance.CurrentCollection++;
            }
            ObjectManager.Instance.CollectionHandler?.Invoke();
            EffectManager.Instance.PlayOneShot(EffectEnum.Collect, transform.position);
            PlaySound();
            ObjectManager.Instance.collections.Remove(this);
            Destroy(gameObject);
            GamePlayManager.Instance.playerController.GetComponent<PlayerEffectController>().StartLeakageIE();
        }
    }
}
