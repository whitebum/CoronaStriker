using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.Types;

namespace CoronaStriker.Core.Actors
{
    /// <summary>
    /// 엑터의 체력 시스템.
    /// </summary>
    public class ActorHealth : MonoBehaviour
    {
        #region Properites
        /// <summary>
        /// 액터의 최대 체력.
        /// </summary>
        [SerializeField] protected int _maxHealth;

        /// <summary>
        /// 액터의 최대 체력.
        /// </summary>
        public virtual int maxHealth
        {
            get { return _maxHealth; }
        }

        /// <summary>
        /// 액터의 현재 체력.
        /// </summary>
        protected int _curHealth;

        /// <summary>
        /// 액터의 현재 체력.
        /// </summary>
        public virtual int curHealth
        {
            get => _curHealth;
            set
            {
                _curHealth = (value < maxHealth) ? value : maxHealth;

                onValueChanged.Invoke();
                if (_curHealth > 0)
                {
                    
                }
                else
                {
                    _curHealth = 0;
                    onDead.Invoke();
                    gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 무적인가에 대한 여부.
        /// </summary>
        [Tooltip("")]
        protected bool isInvincible;

        /// <summary>
        /// 무적 유지 시간.
        /// </summary>
        [Tooltip("무적 유지 시간.")]
        protected float invincibleTimer;

        /// <summary>
        /// 사망했는가에 대한 여부.
        /// </summary>
        [Tooltip("사망했는가에 대한 여부.")]
        protected bool isDead;
        #endregion

        #region Events
        /// <summary>
        /// 체력이 변경되었을 때 실행되는 Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("체력이 변경되었을 때 실행되는 Unity Event.")]
        [HideInInspector] public UnityEvent onValueChanged;

        /// <summary>
        /// 사망했을 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("사망했을 때 실행되는 Unity Event.")]
        [HideInInspector] public UnityEvent onDead;
        #endregion

        protected virtual void Awake()
        {
            onValueChanged = onValueChanged ?? new UnityEvent();
            onDead = onDead ?? new UnityEvent();

            _curHealth = _maxHealth;

            isDead = false;

            isInvincible = false;
            invincibleTimer = 0.0f;
        }

        protected virtual void Update()
        {
            var deltaTime = Time.deltaTime;

            if (isDead) return;
            if (isInvincible)
            {
                if ((invincibleTimer -= deltaTime) <= 0.0f)
                {
                    isInvincible = false;
                    invincibleTimer = 0.0f;
                }
            }
        }

        /// <summary>
        /// 액터의 무적 상태를 활성화합니다.
        /// </summary>
        /// <param name="invincibleTime">무적 유지 시간.</param>
        public virtual void SetInvincible(float invincibleTime)
        {
            isInvincible = true;
            invincibleTimer = invincibleTime;
        }

        /// <summary>
        /// 액터를 무적 상태로 만듭니다.
        /// </summary>
        /// <param name="invincibleTime">무적 유지 시간.</param>
        public virtual void Dead()
        {
            isDead = true;
            if (onDead != null)
                onDead.Invoke();

            gameObject.SetActive(false);
        }
    }
}