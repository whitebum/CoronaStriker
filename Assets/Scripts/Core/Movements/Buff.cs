using CoronaStriker.Core.Actors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.Core.Movements
{
    /// <summary>
    /// 플레이어가 가질 수 있는 여러 버프들을 구현하는 클래스들의 기본 클래스.
    /// </summary>
    public abstract class Buff : MonoBehaviour
    {
        protected BuffManager manager;
        protected PlayerHealth health;


        /// <summary>
        /// 버프가 지속되는 시간.
        /// </summary>
        [Tooltip("버프가 지속되는 시간.")]
        [SerializeField] private float duration;

        /// <summary>
        /// 버프의 지속 시간을 집계하는 타이머.
        /// </summary>
        private float durationTimer;

        #region Events
        /// <summary>
        /// 버프가 시작되었을 때 실행되는 Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("버프가 시작되었을 때 실행되는 Unity Event.")]
        public UnityEvent onStart;

        /// <summary>
        /// 버프가 종료되었을 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("버프가 종료되었을 때 실행되는 Unity Event.")]
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