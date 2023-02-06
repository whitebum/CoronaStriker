using CpronaStriker.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.HUD
{
    /// <summary>
    /// 'Title Scene'에 띄워질 백그라운드 이미지.
    /// </summary>
    public sealed class TitleBackground : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        /// <summary>
        /// 인트로 애니메이션의 트리거.
        /// </summary>
        [Space]
        [Tooltip("인트로 애니메이션의 트리거.")]
        [SerializeField] private string introTrigger;
        private int introTriggerHash;

        /// <summary>
        /// 메인 타이틀 애니메이션의 트리거.
        /// </summary>
        [Tooltip("메인 타이틀 애니메이션의 트리거.")]
        [SerializeField] private string titleTrigger;
        private int titleTriggerHash;

        /// <summary>
        /// 현재 백그라운드 이미지의 상태를 나타내는 열거형.
        /// </summary>
        public enum BackgroundState
        {
            None,
            Introduction,
            MainTitle,
        }

        /// <summary>
        /// 현재 백그라운드 이미지의 상태.
        /// </summary>
        [HideInInspector] public BackgroundState currentState;

        private Coroutine coroutine;

        private void Reset()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Awake()
        {
            introTriggerHash = !string.IsNullOrEmpty(introTrigger) ? introTriggerHash : 0;
            titleTriggerHash = !string.IsNullOrEmpty(titleTrigger) ? titleTriggerHash : 0;
            
            currentState = BackgroundState.None;
        }
    }
}