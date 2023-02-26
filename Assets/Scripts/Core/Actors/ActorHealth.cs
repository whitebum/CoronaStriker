using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.Types;

namespace CoronaStriker.Core.Actors
{
    /// <summary>
    /// ������ ü�� �ý���.
    /// </summary>
    public class ActorHealth : MonoBehaviour
    {
        #region Properites
        /// <summary>
        /// ������ �ִ� ü��.
        /// </summary>
        [SerializeField] protected int _maxHealth;

        /// <summary>
        /// ������ �ִ� ü��.
        /// </summary>
        public virtual int maxHealth
        {
            get { return _maxHealth; }
        }

        /// <summary>
        /// ������ ���� ü��.
        /// </summary>
        protected int _curHealth;

        /// <summary>
        /// ������ ���� ü��.
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
        /// �����ΰ��� ���� ����.
        /// </summary>
        [Tooltip("")]
        protected bool isInvincible;

        /// <summary>
        /// ���� ���� �ð�.
        /// </summary>
        [Tooltip("���� ���� �ð�.")]
        protected float invincibleTimer;

        /// <summary>
        /// ����ߴ°��� ���� ����.
        /// </summary>
        [Tooltip("����ߴ°��� ���� ����.")]
        protected bool isDead;
        #endregion

        #region Events
        /// <summary>
        /// ü���� ����Ǿ��� �� ����Ǵ� Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("ü���� ����Ǿ��� �� ����Ǵ� Unity Event.")]
        [HideInInspector] public UnityEvent onValueChanged;

        /// <summary>
        /// ������� �� ����Ǵ� Unity Event.
        /// </summary>
        [Tooltip("������� �� ����Ǵ� Unity Event.")]
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
        /// ������ ���� ���¸� Ȱ��ȭ�մϴ�.
        /// </summary>
        /// <param name="invincibleTime">���� ���� �ð�.</param>
        public virtual void SetInvincible(float invincibleTime)
        {
            isInvincible = true;
            invincibleTimer = invincibleTime;
        }

        /// <summary>
        /// ���͸� ���� ���·� ����ϴ�.
        /// </summary>
        /// <param name="invincibleTime">���� ���� �ð�.</param>
        public virtual void Dead()
        {
            isDead = true;
            if (onDead != null)
                onDead.Invoke();

            gameObject.SetActive(false);
        }
    }
}