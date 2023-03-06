using CoronaStriker.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    public sealed class TitleBackground : MonoBehaviour
    {
        public enum BackgroundState
        {
            None,
            Opening,
            MainTitle,
        }

        public BackgroundState currentState;

        /// <summary>
        /// 
        /// </summary>
        [Header("Animations")]
        [Tooltip("")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// 
        /// </summary>
        [Space]
        [Tooltip("")]
        [SerializeField] private string openingTriggerName;
        private int openingTrigger;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private string mainTitleTriggerName;
        private int mainTitleTrigger;

        /// <summary>
        /// 오프닝 애니메이션에서 메인 타이틀 애니메이션까지의 Transition Wait Module.
        /// </summary>
        [Header("Modules")]
        [Tooltip("오프닝 애니메이션에서 메인 타이틀 애니메이션까지의 Transition Wait Module.")]
        [SerializeField] private TransitionWaitModule openingToMainTitle;

        /// <summary>
        /// 
        /// </summary>
        private Coroutine transitionCoroutine;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            currentState = BackgroundState.None;

            animator = animator ?? GetComponent<Animator>();

            openingTrigger = !string.IsNullOrEmpty(openingTriggerName) ? Animator.StringToHash(openingTriggerName) : 0;
            mainTitleTrigger = !string.IsNullOrEmpty(mainTitleTriggerName) ? Animator.StringToHash(mainTitleTriggerName) : 0;
        }

        private void Start()
        {
            PlayOpeningAnimation();
        }

        public void PlayOpeningAnimation()
        {
            StopAnimationCoroutine();

            currentState = BackgroundState.Opening;
            if (animator != null && openingTrigger != 0)
            {
                animator.SetTrigger(openingTrigger);
                animator.Update(0.0f);
            }

            if (openingToMainTitle != default)
                StartCoroutine(WaitForAnimationCoroutine(openingToMainTitle, PlayMainTitleOpening));
        }

        public void PlayMainTitleOpening()
        {
            StopAnimationCoroutine();

            currentState = BackgroundState.MainTitle;
            if (animator != null && mainTitleTrigger != 0)
            {
                animator.SetTrigger(mainTitleTrigger);
                animator.Update(0.0f);
            }
        }

        public void SkipOpeningAnimation()
        {
            if (currentState == BackgroundState.Opening)
            {
                StopAnimationCoroutine();
                PlayMainTitleOpening();
            }
        }

        private void StopAnimationCoroutine()
        {
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = null;
            }
        }

        private IEnumerator WaitForAnimationCoroutine(TransitionWaitModule module, UnityAction action)
        {
            if (module != default)
            {
                if (!string.IsNullOrEmpty(module.animation) && animator != null)
                {
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f &&
                                                     animator.GetCurrentAnimatorStateInfo(0).IsName(module.animation));
                }
                else if (module.time <= 0.0f)
                {
                    yield return new WaitForSeconds(module.time);
                }
                else if (module.audio)
                {
                    yield return null;
                }
            }

            yield return null;
        }
    }
}
