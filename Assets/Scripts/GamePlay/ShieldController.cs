using Runner.GamePlay.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.GamePlay
{
    public class ShieldController : MonoBehaviour
    {
        public PlayerController playerController;
        public bool canKick = false;
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag("FieldObject"))
            {
                var baseFieldObj = collision.gameObject.GetComponent<KickObstacle>();
                if (baseFieldObj == null) return;
                if (canKick)
                {
                    baseFieldObj.OnCollision();
                    canKick = false;
                }
                else
                {
                    GamePlayManager.Instance.AddLife(baseFieldObj.damageValue);
                    GamePlayManager.Instance.AddScore(baseFieldObj.scoreValue);
                    playerController.Hurt();
                }
            }
        }
    }

}
