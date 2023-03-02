using CoronaStriker.Core.Actors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.Core.Movements
{
    /// <summary>
    /// �÷��̾ ���� �� �ִ� ���� �������� �����ϴ� Ŭ�������� �⺻ Ŭ����.
    /// </summary>
    public abstract class Buff : MonoBehaviour
    {
        protected BuffManager manager;
        protected PlayerHealth health;


        /// <summary>
        /// ������ ���ӵǴ� �ð�.
        /// </summary>
        [Tooltip("������ ���ӵǴ� �ð�.")]
        [SerializeField] private float duration;

        /// <summary>
        /// ������ ���� �ð��� �����ϴ� Ÿ�̸�.
        /// </summary>
        private float durationTimer;

        #region Events
        /// <summary>
        /// ������ ���۵Ǿ��� �� ����Ǵ� Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("������ ���۵Ǿ��� �� ����Ǵ� Unity Event.")]
        public UnityEvent onStart;

        /// <summary>
        /// ������ ����Ǿ��� �� ����Ǵ� Unity Event.
        /// </summary>
        [Tooltip("������ ����Ǿ��� �� ����Ǵ� Unity Event.")]
        public UnityEvent onEnd;
        #endregion

        protected virtual void Awake()
        {
            onStart = onStart ?? new UnityEvent();
            onEnd = onEnd ?? new UnityEvent();
        }

        private void Update()
        {
            if (duration > 0.0f && 
               (durationTimer += Time.deltaTime) >= duration)
            {
                duration = 0.0f;
                durationTimer = 0.0f;

                OnEnd();
            }
        }

        public virtual void OnStart()
        {
            if (manager != null)
            {
                manager.AddBuff(this);

                onStart.Invoke();
            }
        }

        public virtual void OnEnd()
        {
            if (manager != null)
            {
                manager.RemoveBuff(this);
            }
        }
    }
}