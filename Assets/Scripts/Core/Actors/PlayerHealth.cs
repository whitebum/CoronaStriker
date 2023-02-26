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
        /// �ǰݵǾ��°��� ���� ����.
        /// </summary>
        [Tooltip("�ǰݵǾ��°��� ���� ����.")]
        private bool isHurt;

        /// <summary>
        /// �ǰݵǾ��� �� ���� ���� �ð�.
        /// </summary>
        [Tooltip("�ǰݵǾ��� �� ���� ���� �ð�.")]
        [SerializeField] private float hurtInvincibleTime;

        /// <summary>
        /// �ǰݵǾ��� ���� ���� ���� �ð��� üũ�ϴ� Ÿ�̸�.
        /// </summary>
        [Tooltip("�ǰݵǾ��� ���� ���� ���� �ð��� üũ�ϴ� Ÿ�̸�.")]
        private float hurtInvincibleTimer;
        #endregion

        #region Animations
        /// <summary>
        /// �ִϸ��̼��� ����ϴ� Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("�ִϸ��̼��� ����ϴ� Animator.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// ü�¿� ���� �ִϸ��̼��� �����ϴ� ���̾��� �̸�.
        /// </summary>
        [Space]
        [Tooltip("ü�¿� ���� �ִϸ��̼��� �����ϴ� ���̾��� �̸�.")]
        [SerializeField] private string healthLayerName;

        /// <summary>
        /// ü�¿� ���� �ִϸ��̼��� �����ϴ� ���̾��� �ε���.
        /// </summary>
        private int healthLayerIndex;

        /// <summary>
        /// �ǰ� �ִϸ��̼��� �����ϴ� ���̾��� �̸�.
        /// </summary>
        [Tooltip("�ǰ� �ִϸ��̼��� �����ϴ� ���̾��� �̸�.")]
        [SerializeField] private string hurtLayerName;

        /// <summary>
        /// �ǰ� �ִϸ��̼��� �����ϴ� ���̾��� �ε���.
        /// </summary>
        private int hurtLayerIndex;

        /// <summary>
        /// ü�¿� ���� �ִϸ��̼��� ����ϴ� Integer �Ķ����.
        /// </summary>
        [Space]
        [Tooltip("ü�¿� ���� �ִϸ��̼��� ����ϴ� Integer �Ķ����.")]
        [SerializeField] private string healthInt;

        /// <summary>
        /// ü�¿� ���� �ִϸ��̼��� ����ϴ� Integer �Ķ���� �ؽ�.
        /// </summary>
        private int healthIntHash;

        /// <summary>
        /// �ǰ� �ִϸ��̼��� ����ϴ� Boolean �Ķ����.
        /// </summary>
        [Tooltip("�ǰݵǾ��� �� ���� ���� �ð�.")]
        [SerializeField] private string hurtBoolen;

        /// <summary>
        /// �ǰ� �ִϸ��̼��� ����ϴ� Boolean �Ķ���� �ؽ�.
        /// </summary>
        private int hurtBoolenHash;

        /// <summary>
        /// ��� �ִϸ��̼��� ����ϴ� Trigger �Ķ����.
        /// </summary>
        [Tooltip("�ǰݵǾ��� �� ���� ���� �ð�.")]
        [SerializeField] private string deadTrigger;

        /// <summary>
        /// ��� �ִϸ��̼��� ����ϴ� Trigger �Ķ���� �ؽ�.
        /// </summary>
        private int deadTriggerHash;

        /// <summary>
        /// �ǰ� �ִϸ��̼� ��⿡ ���Ǵ� Coroutine.
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
        /// �ǰ� ȿ���� ����մϴ�.
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
        /// ������ ��� ó���� �����մϴ�.
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
        /// �ִϸ��̼��� �����մϴ�.
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
        /// �ִϸ��̼��� ����� ������ ����ϴ� �ڷ�ƾ.
        /// </summary>
        /// <param name="animation">Ÿ�� �ִϸ��̼�.</param>
        /// <param name="callback">��� �� �����ų �ݹ�.</param>
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
