using CoronaStriker.Core.Utils;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.Core.Actors
{
    public sealed class PlayerHealth : ActorHealth
    {
        #region Properties
        public override int curHealth 
        {
            get => base.curHealth;
            set
            {
                _curHealth = (value < maxHealth) ? value : maxHealth;

                isHurt = true;
                hurtInvincibleTimer = hurtInvincibleTime;   

                onValueChanged.Invoke();
                if (_curHealth > 0)
                {
                    if (animator != null && healthIntHash != 0)
                    {
                        animator.SetInteger(healthIntHash, _curHealth);
                        animator.Update(animator.GetCurrentAnimatorStateInfo(healthLayerIndex).normalizedTime);
                    }

                    Hurt();
                }
                else
                {
                    Dead();
                }
            }
        }

        /// <summary>
        /// 피격되었는가에 대한 여부.
        /// </summary>
        [Tooltip("피격되었는가에 대한 여부.")]
        private bool isHurt;

        /// <summary>
        /// 피격되었을 때 무적 유지 시간.
        /// </summary>
        [Tooltip("피격되었을 때 무적 유지 시간.")]
        [SerializeField] private float hurtInvincibleTime;

        /// <summary>
        /// 피격되었을 때의 무적 유지 시간을 체크하는 타이머.
        /// </summary>
        [Tooltip("피격되었을 때의 무적 유지 시간을 체크하는 타이머.")]
        private float hurtInvincibleTimer;
        #endregion

        #region Animations
        /// <summary>
        /// 애니메이션을 재생하는 Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("애니메이션을 재생하는 Animator.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// 체력에 따른 애니메이션을 관리하는 레이어의 이름.
        /// </summary>
        [Space]
        [Tooltip("체력에 따른 애니메이션을 관리하는 레이어의 이름.")]
        [SerializeField] private string healthLayerName;

        /// <summary>
        /// 체력에 따른 애니메이션을 관리하는 레이어의 인덱스.
        /// </summary>
        private int healthLayerIndex;

        /// <summary>
        /// 피격 애니메이션을 관리하는 레이어의 이름.
        /// </summary>
        [Tooltip("피격 애니메이션을 관리하는 레이어의 이름.")]
        [SerializeField] private string hurtLayerName;

        /// <summary>
        /// 피격 애니메이션을 관리하는 레이어의 인덱스.
        /// </summary>
        private int hurtLayerIndex;

        /// <summary>
        /// 체력에 따른 애니메이션을 재생하는 Integer 파라미터.
        /// </summary>
        [Space]
        [Tooltip("체력에 따른 애니메이션을 재생하는 Integer 파라미터.")]
        [SerializeField] private string healthInt;

        /// <summary>
        /// 체력에 따른 애니메이션을 재생하는 Integer 파라미터 해쉬.
        /// </summary>
        private int healthIntHash;

        /// <summary>
        /// 피격 애니메이션을 재생하는 Boolean 파라미터.
        /// </summary>
        [Tooltip("피격되었을 때 무적 유지 시간.")]
        [SerializeField] private string hurtBoolen;

        /// <summary>
        /// 피격 애니메이션을 재생하는 Boolean 파라미터 해쉬.
        /// </summary>
        private int hurtBoolenHash;

        /// <summary>
        /// 사망 애니메이션을 재생하는 Trigger 파라미터.
        /// </summary>
        [Tooltip("피격되었을 때 무적 유지 시간.")]
        [SerializeField] private string deadTrigger;

        /// <summary>
        /// 사망 애니메이션을 재생하는 Trigger 파라미터 해쉬.
        /// </summary>
        private int deadTriggerHash;

        /// <summary>
        /// 피격 애니메이션 대기에 사용되는 Coroutine.
        /// </summary>
        private Coroutine animationCoroutine;
        #endregion

        private void Reset()
        {
            animator = GetComponentInChildren<Animator>();
        }

        protected override void Awake()
        {
            base.Awake();

            animator = animator ?? GetComponentInChildren<Animator>();

            healthLayerIndex = !string.IsNullOrEmpty(healthLayerName) ? animator.GetLayerIndex(healthLayerName) : -1;
            hurtLayerIndex = !string.IsNullOrEmpty(hurtLayerName) ? animator.GetLayerIndex(hurtLayerName) : -1;

            healthIntHash = !string.IsNullOrEmpty(healthInt) ? Animator.StringToHash(healthInt) : 0;
            hurtBoolenHash = !string.IsNullOrEmpty(hurtBoolen) ? Animator.StringToHash(hurtBoolen) : 0;
            deadTriggerHash = !string.IsNullOrEmpty(deadTrigger) ? Animator.StringToHash(deadTrigger) : 0;

            isHurt = false;
            hurtInvincibleTimer = 5.0f;
        }

        protected override void Update()
        {
            if (isDead == false)
            {
                if (isHurt == true && (hurtInvincibleTimer -= Time.deltaTime) <= 0.0f)
                {
                    isHurt = false;
                    hurtInvincibleTimer = 0.0f;

                    if (animator != null && hurtBoolenHash != 0)
                    {
                        animator.SetBool(hurtBoolenHash, false);
                        animator.Update(0.0f);
                    }
                }
                if (isInvincible == true && (invincibleTimer -= Time.deltaTime) <= 0.0f)
                {
                    Debug.Log("??");

                    isInvincible = false;
                    invincibleTimer = 0.0f;
                }

                if (Input.GetKeyDown(InputUtility.selectKey))
                {
                    Hurt();
                }
            }
        }

        public override void SetInvincible(float invincibleTime)
        {
            isInvincible = true;
            invincibleTimer = invincibleTime;

            //if (isHurt == true)
            //{
            //    if (animator != null && hurtBoolenHash != 0)
            //    {
            //        if (animator.GetBool(hurtBoolenHash) == true)
            //            animator.SetBool(hurtBoolenHash, false);
            //    }
            //}
        }

        /// <summary>
        /// 피격 효과를 재생합니다.
        /// </summary>
        public void Hurt()
        {
            isHurt = true;

            if (animator != null && hurtBoolenHash != 0)
            {
                animator.SetBool(hurtBoolenHash, true);
                animator.Update(0.0f);
            }
        }

        /// <summary>
        /// 액터의 사망 처리를 진행합니다.
        /// </summary>
        public override void Dead()
        {
            onDead.Invoke();

            if (isHurt == true) hurtInvincibleTimer = 0.0f;
            if (animator != null && deadTriggerHash != 0)
            {
                animator.SetTrigger(deadTriggerHash);
                animator.Update(0.0f);
            }

            StopAnimationCoroutine();
            animationCoroutine = StartCoroutine(WaitForAnimationCoroutine(deadTrigger, () => gameObject.SetActive(false)));
        }

        /// <summary>
        /// 애니메이션을 중지합니다.
        /// </summary>
        private void StopAnimationCoroutine()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }

        /// <summary>
        /// 애니메이션이 종료될 때까지 대기하는 코루틴.
        /// </summary>
        /// <param name="animation">타겟 애니메이션.</param>
        /// <param name="callback">대기 후 실행시킬 콜백.</param>
        /// <returns></returns>
        private IEnumerator WaitForAnimationCoroutine(string animation, UnityAction callback)
        {
            if (animator != null && !string.IsNullOrEmpty(animation))
            {
                if (healthLayerIndex != -1)
                {
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(healthLayerIndex).normalizedTime >= 0.999f &&
                                                     animator.GetCurrentAnimatorStateInfo(healthLayerIndex).IsName(animation));
                }
            }

            callback.Invoke();

            yield return null;
        }
    }
}
