using CpronaStriker.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.HUD
{
    /// <summary>
    /// 'Title Scene'�� ����� ��׶��� �̹���.
    /// </summary>
    public sealed class TitleBackground : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        /// <summary>
        /// ��Ʈ�� �ִϸ��̼��� Ʈ����.
        /// </summary>
        [Space]
        [Tooltip("��Ʈ�� �ִϸ��̼��� Ʈ����.")]
        [SerializeField] private string introTrigger;
        private int introTriggerHash;

        /// <summary>
        /// ���� Ÿ��Ʋ �ִϸ��̼��� Ʈ����.
        /// </summary>
        [Tooltip("���� Ÿ��Ʋ �ִϸ��̼��� Ʈ����.")]
        [SerializeField] private string titleTrigger;
        private int titleTriggerHash;

        /// <summary>
        /// ���� ��׶��� �̹����� ���¸� ��Ÿ���� ������.
        /// </summary>
        public enum BackgroundState
        {
            None,
            Introduction,
            MainTitle,
        }

        /// <summary>
        /// ���� ��׶��� �̹����� ����.
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