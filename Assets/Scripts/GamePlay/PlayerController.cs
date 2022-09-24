using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using Runner.GamePlay.Objects;
using Runner.Core;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Runner.Utils;

namespace Runner.GamePlay
{
    public class PlayerController : MonoBehaviour
    {
        private Volume globalVolume;
        public float volumeTweenTime = 2f;
        private Tween volumeTween;


        private PlayerControls playerControls;
        private Rigidbody rb;
        private Animator anim;
        private PlayerGravityController gravityController;        

        public float jumpForce=2f;

        private OnValueChangedEventListener<bool> isGrouded;
        public float groundCheckRadius=0.1f;
        public Transform checkBoxTrans;

        private float maxSpeed;

        private GameObject kickShield;
        private bool shielding;
        public float shieldTime = 0.5f;

        private bool dead;

        private Vector3 originPos;

        public Material hurtMaterial;
        private Dictionary<string, Material[]> originMaterialDic;
        public float flushTime = 0.2f;

        private bool isFlying;
        private bool canFlyMove;
        //private Tween flyMoveTween;

        [Header("喷气背包prefab")]
        public GameObject bag;

        [Header("空中移动所用时间")]
        public float flyMoveTweenTime=0.5f;
        [Header("空中最高高度")]
        public float flyHeight=2f;

        private int flyIndex;

        [Header("滑铲CD")]
        public float slideCD = 5f;
        private Coroutine slideCDCor;

        public ParticleSystem trailParticle;

        private PlayerEffectController effectController;

        private void Awake()
        {
            playerControls = new PlayerControls();
            rb = GetComponent<Rigidbody>();
            anim=GetComponent<Animator>();
            gravityController = gameObject.GetComponent<PlayerGravityController>();
            effectController = GetComponent<PlayerEffectController>();
            kickShield = gameObject.transform.Find("KickShield").gameObject;
            globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
            
            ChromaticAberration chro;
            if(globalVolume.profile.TryGet<ChromaticAberration>(out chro))
            {
                volumeTween = DOTween.To(() => chro.intensity.value, x => chro.intensity.value = x, 0.6f, volumeTweenTime).SetAutoKill(false).Pause().OnComplete(() =>
                {
                    volumeTween.PlayBackwards();
                });
            }           
        }

        private void OnDestroy()
        {
            GamePlayManager.Instance.VictoryAction -= MissionSuccess;
            GamePlayManager.Instance.RetryAction -= Reborn;
        }

        void Start()
        {
            GamePlayManager.Instance.VictoryAction += MissionSuccess;
            GamePlayManager.Instance.RetryAction += Reborn;
            isGrouded = new OnValueChangedEventListener<bool>();           
            canFlyMove = false;
            shielding = false;
            flyIndex = 0;
            isFlying = false;
            maxSpeed = 1f;
            dead = false;
            originPos = transform.position;
            originMaterialDic = new Dictionary<string, Material[]>();
            //flyMoveTween = null;
            bag.SetActive(false);
            SaveOriginMaterialInfo();

            var originPosY = transform.position.y+FlyManager.Instance.height;          

            playerControls.GamePlay.Up.performed += ctx =>
            {
                if(isFlying)
                {
                    if (!canFlyMove) return;
                    //if (flyMoveTween!=null&&flyMoveTween.IsPlaying()) return;
                    FlyTo(true);
                }
            };

            playerControls.GamePlay.Slide.performed += ctx =>
            {
                if (isFlying)
                {
                    if (!canFlyMove) return;
                    //if (flyMoveTween != null && flyMoveTween.IsPlaying()) return;
                    FlyTo(false);
                }
                else
                {
                    if (FlyManager.Instance.backSequence.IsPlaying()) return;

                    ResetTrigger();
                    anim.SetBool("Slide", true);
                    gravityController.G = -50f;
                    trailParticle.Stop();

                    if (slideCDCor != null) StopCoroutine(slideCDCor);
                    slideCDCor = StartCoroutine(SlideCDIE());
                }   
            };

            playerControls.GamePlay.Slide.canceled += ctx =>
            {
                if(isFlying)
                {

                }
                else
                {
                    if(slideCDCor!=null) StopCoroutine(slideCDCor);
                    CancelSlide();            
                }                    
            };

            playerControls.GamePlay.Jump.performed += ctx =>
            {
                if (isFlying) return;
                if (!isGrouded.Value) return;
                ResetTrigger();
                anim.SetTrigger("Jump");
                ObjectManager.Instance.PlayJumpSound();
                rb.AddForce(Vector3.up*jumpForce);
                DOVirtual.DelayedCall(Time.fixedDeltaTime+0.1f, () =>
                {
                    maxSpeed = rb.velocity.y;                    
                });                
            };

            playerControls.GamePlay.Kick.performed += ctx =>
            {
                if (isFlying) return;
                ResetTrigger();
                anim.SetTrigger("Kick");
                if (shielding) return;
                StartCoroutine(ShieldIE());
                
            };            

            isGrouded.OnValueChangedEvent += newValue =>
            {
                anim.SetBool("IsGround", newValue);   
                if(newValue)
                {
                    gravityController.G = -40f;
                    if (anim.GetBool("Slide")) return;
                    trailParticle.Play();
                }
                else
                {
                    trailParticle.Stop();
                }
            };
            gravityController.G = -40f;

            if (!GameManager.Instance.isEditor)
            {
                GamePlayManager.Instance.playerHealth = new() { Value = 100 };
                GamePlayManager.Instance.playerHealth.OnValueChangedEvent += newValue =>
                {
                    //角色死亡 重新加载关卡
                    if (newValue <= 0 && !dead)
                    {
                        anim.SetTrigger("Die");
                        dead = true;
                        LoopSceneManager.Instance.Pause();

                        DOVirtual.DelayedCall(1f, () =>
                        {
                            Reborn();
                            GamePlayManager.Instance.Retry();
                            dead = false;
                        });
                    }
                };
            }
        }

        void FixedUpdate()
        {            
            anim.SetFloat("SpeedY", rb.velocity.y/maxSpeed);
            if(!isGrouded.Value&&!isFlying)
            {
                var vel=rb.velocity;
                vel += new Vector3(0, gravityController.G*Time.fixedDeltaTime, 0);
                rb.velocity = vel;
            }
            GroudCheck();         
        }

        private void GroudCheck()
        {
            var res = Physics.OverlapSphere(checkBoxTrans.position, groundCheckRadius,LayerMask.GetMask("Ground")).Length>0?true:false;
            isGrouded.Value = res;
        }

        public void EnablePlayerController()
        {
            playerControls?.GamePlay.Enable();
        }

        private void OnDisable()
        {
            playerControls?.GamePlay.Disable();
        }

        private IEnumerator ShieldIE()
        {
            kickShield.SetActive(true);
            shielding = true;
            kickShield.GetComponent<ShieldController>().canKick = true;
            yield return new WaitForSeconds(shieldTime);
            kickShield.SetActive(false);
            shielding = false;
            kickShield.GetComponent<ShieldController>().canKick = false;
        }

        public void Reborn()
        {
            playerControls?.GamePlay.Enable();
            anim.ResetTrigger("Die");
            anim.ResetTrigger("Victory");

            anim.SetBool("Flying", false);
            anim.SetBool("Slide", false);
            transform.position = originPos;
            anim.Play("Run", 0);
            trailParticle.Play();
        }
        [ContextMenu("Hurt")]
        public void Hurt()
        {
            CameraShake();
            volumeTween.Restart();
            StartCoroutine(HurtFlushIE());
            ObjectManager.Instance.PlayHurtSound();
        }

        private IEnumerator HurtFlushIE()
        {
            ObjectManager.Instance.DecreaseGameSpeed();
            GamepadVibrateManager.Instance.Vibrate(VibrateEnum.LongStrong);
            ChangeMaterial(hurtMaterial);
            yield return new WaitForSeconds(flushTime);
            RestoreMaterial();
        }

        private IEnumerator HurtPostProcessingIE()
        {

            yield return new WaitForSeconds(flushTime);
        }


        //保存原始材质数据
        private void SaveOriginMaterialInfo()
        {
            foreach (var v in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                originMaterialDic.Add(v.name, v.materials);
            }
        }

        //改变材质
        private void ChangeMaterial(Material mat)
        {
            foreach (var v in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                v.material = mat;
            }
        }

        //恢复材质
        private void RestoreMaterial()
        {
            foreach (var v in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                v.materials = originMaterialDic[v.name];
            }
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (dead) return;
            if (collision.transform.CompareTag("FieldObject"))
            {
                var baseFieldObj = collision.gameObject.GetComponent<BaseFieldObject>();
                bool isKick = baseFieldObj is KickObstacle;

                if (baseFieldObj == null || (shielding && isKick)) return;

                if (baseFieldObj is not BaseObstacle)
                {
                    (baseFieldObj as ICollisionableObject).OnCollision();
                }

                if (baseFieldObj.damageValue < 0)
                {
                    GamePlayManager.Instance.AddLife(baseFieldObj.damageValue * GamePlayManager.Instance.HealthMultiply);
                }
                else
                {
                    GamePlayManager.Instance.AddLife(baseFieldObj.damageValue / GamePlayManager.Instance.HealthMultiply);
                }

                GamePlayManager.Instance.AddScore(baseFieldObj.scoreValue);

                if (baseFieldObj.damageValue < 0)
                {
                    Hurt();
                }
            }
        }   
        
        public void OnFlyToTopTween()
        {
            rb.velocity = Vector3.zero;
            isFlying = true;
            bag.SetActive(true);
            anim.SetBool("Flying", true);
            effectController.rainbowTrail.Play();
        }

        public void OnFlyToTopTweenComplete()
        {
            canFlyMove = true;
        }

        public void OnBackToBottomTween()
        {
            isFlying = false;
            canFlyMove = false;
            flyIndex = 0;
            //flyMoveTween = null;
            anim.SetBool("Flying", false);
            effectController.rainbowTrail.Stop();
        }

        public void OnBackToBottomTweenComplete()
        {
            bag.SetActive(false);
        }

        public void ResetTrigger()
        {
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("Kick");
        }

        public void CameraShake()
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }

        private void FlyTo(bool up)
        {
           if(up)
           {
                if (flyIndex >= 2) return;
                FlyTween(1);
                flyIndex += 1;
           }
           else
           {
                if (flyIndex <= 0) return;
                FlyTween(-1);
                flyIndex -= 1;
            }
        }

        private void FlyTween(int dir)
        {
            var startPosY = transform.position.y;
            canFlyMove = false;
            transform.DOMoveY(startPosY+ flyHeight/2*dir, flyMoveTweenTime).SetEase(Ease.Linear).onComplete+=()=>
            {
                canFlyMove=true;
            };
        }

        private IEnumerator SlideCDIE()
        {
            yield return new WaitForSeconds(slideCD);
            CancelSlide();
        }

        private void CancelSlide()
        {
            anim.SetBool("Slide", false);
            foreach (var v in effectController.sparks) v.gameObject.SetActive(false);
            trailParticle.Play();
        }

        private void MissionSuccess()
        {
            playerControls?.GamePlay.Disable();
            anim.SetTrigger("Victory");
            trailParticle.Stop();
            LoopSceneManager.Instance.Pause();
        }
    }
}

