using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Runner.Utils;

namespace Runner.GamePlay.Objects
{
    public class KickObstacle : BaseObstacle, ICollisionableObject
    {
        public void OnCollision()
        {
            GamepadVibrateManager.Instance.Vibrate(VibrateEnum.ShortStrong);
            EffectManager.Instance.PlayOneShot(EffectEnum.Burst, transform.position+new Vector3(0,0.7f,0));
            PlaySound();
            ObjectManager.Instance.obstacles.Remove(this);
            Destroy(gameObject);
        }

        public override void PlaySound()
        {
            ObjectManager.Instance.PlayKickSound();
        }

    }
}
