using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class Transition : MonoBehaviour
    {
        #region Animations
        /// <summary>
        /// 애니메이션을 재생하는 Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("")]
        [SerializeField] protected Animator animator;

        /// <summary>
        /// 오브젝트가 열릴 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Space]
        [Tooltip("오브젝트가 열릴 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string openTrigger;

        /// <summary>
        /// 오브젝트가 열릴 때 재생되는 애니메이션의 Trigger 파라미터의 해쉬값.
        /// </summary>
        protected int openTriggerHash;

        /// <summary>
        /// 오브젝트가 열렸을 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Tooltip("오브젝트가 열렸을 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string openCompleteTrigger;

        /// <summary>
        /// 오브젝트가 열렸을 때 재생되는 애니메이션의 Trigger 파라미터의 해쉬값.
        /// </summary>
        protected int openCompleteTriggerHash;

        /// <summary>
        /// 오브젝트가 닫힐 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Tooltip("오브젝트가 닫힐 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string closeTrigger;

        /// <summary>
        /// 오브젝트가 닫힐 때 재생되는 애니메이션의 Trigger 파라미터의 해쉬값.
        /// </summary>
        protected int closeTriggerHash;

        /// <summary>
        /// 오브젝트가 닫혔을 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Tooltip("오브젝트가 열릴 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string closeCompleteTrigger;

        /// <summary>
        /// 오브젝트가 닫혔을 때 재생되는 애니메이션의 Trigger 파라미터의 해쉬값.
        /// </summary>
        protected int closeCompleteTriggerHash;
        #endregion

        #region Modules
        /// <summary>
        /// 오브젝트가 완전히 열릴 때까지 대기하는 Transition Wait Module.
        /// </summary>
        [Header("Modules")]
        [Tooltip("오브젝트가 열릴 때까지 대기하는 Transition Wait Module.")]
        [SerializeField] protected TransitionWaitModule openToOpenComplete;

        /// <summary>
        /// 오브젝트가 닫힐 때까지 대기하는 Transition Wait Module.
        /// </summary>
        [Tooltip("오브젝트가 닫힐 때까지 대기하는 Transition Wait Module.")]
        [SerializeField] protected TransitionWaitModule openCompleteToClose;

        /// <summary>
        /// 오브젝트가 완전히 닫힐 때까지 대기하는 Transition Wait Module.
        /// </summary>
        [Tooltip("오브젝트가 완전히 닫힐 때까지 대기하는 Transition Wait Module.")]
        [SerializeField] protected TransitionWaitModule closeToCloseComplete;
        #endregion

        #region Events
        /// <summary>
        /// 오브젝트가 열릴 때 실행되는 Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("오브젝트가 열릴 때 실행되는 Unity Event.")]
        [HideInInspector] public UnityEvent onOpen;

        /// <summary>
        /// 오브젝트가 열렸을 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("오브젝트가 열렸을 때 실행되는 Unity Event.")]
        [HideInInspector] public UnityEvent onOpenComplete;

        /// <summary>
        /// 오브젝트가 닫힐 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("오브젝트가 닫힐 때 실행되는 Unity Event.")]
        [HideInInspector] public UnityEvent onClose;

        /// <summary>
        /// 오브젝트가 열릴 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("오브젝트가 닫혔을 때 실행되는 Unity Event.")]
        [HideInInspector] public UnityEvent onCloseComplete;
        #endregion

        protected Coroutine transitionCoroutine;

        protected virtual void Reset()
        {
            animator = gameObject.TryGetComponent<Animator>(out var cashe) ? cashe : gameObject.AddComponent<Animator>();
        }

        protected virtual void Awake()
        {
            animator = animator ?? GetComponent<Animator>();

            openTriggerHash = !string.IsNullOrEmpty(openTrigger) ? Animator.StringToHash(openTrigger) : 0;
            openCompleteTriggerHash = !string.IsNullOrEmpty(openCompleteTrigger) ? Animator.StringToHash(openCompleteTrigger) : 0;
            closeTriggerHash = !string.IsNullOrEmpty(closeTrigger) ? Animator.StringToHash(closeTrigger) : 0;
            closeCompleteTriggerHash = !string.IsNullOrEmpty(closeCompleteTrigger) ? Animator.StringToHash(closeCompleteTrigger) : 0;

            onOpen = onOpen ?? new UnityEvent();
            onOpenComplete = onOpenComplete ?? new UnityEvent();
            onClose = onClose ?? new UnityEvent();
            onCloseComplete = onCloseComplete ?? new UnityEvent();
        }

        /// <summary>
        /// 대기 중인 Corotuine을 즉시 중지합니다.
        /// </summary>
        public virtual void StopTransitionCoroutine()
        {
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = null;
            }
        }

        /// <summary>
        /// Transition Wait Module을 대기하고, Unity Action을 실행합니다.
        /// </summary>
        /// <param name="module">대기할 Transition Wait Module.</param>
        /// <param name="action">대기 후 실행할 콜백.</param>
        /// <returns></returns>
        public virtual IEnumerator WaitForTransitionCoroutine(TransitionWaitModule module, UnityAction action)
        {
            if (!string.IsNullOrEmpty(module.animation) && animator != null)
            {
                var curAnimatorInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitUntil(() => curAnimatorInfo.normalizedTime >= 0.999f && curAnimatorInfo.IsName(module.animation));
            }
            else if(module.time <= 0.0f)
            {
                yield return new WaitForSeconds(module.time);
            }
            else if(module.audio != null)
            {
                // 오디오 종료 대기 코드 작성.
            }
            else if(module.transition != null)
            {
                // Transition 대기 코드 작성.
            }
            else
            {
                yield break;
            }
            action.Invoke();

            yield return null;
        }
    }
}
