using CoronaStriker.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.UI
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        #region Animations
        /// <summary>
        /// 애니메이션을 재생하는 Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("애니메이션을 재생하는 Animator.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// 백그라운드가 활성화되는 애니메이션을 재생하는 Trigger 파라미터.
        /// </summary>
        [Space]
        [Tooltip("백그라운드가 활성화되는 애니메이션을 재생하는 Trigger 파라미터.")]
        [SerializeField] private string openTriggerName;
        private int openTrigger;

        /// <summary>
        /// 백그라운드가 비활성화되는 애니메이션을 재생하는 Trigger 파라미터.
        /// </summary>
        [Tooltip("백그라운드가 비활성화되는 애니메이션을 재생하는 Trigger 파라미터.")]
        [SerializeField] private string closeTriggerName;
        private int closeTrigger;
        #endregion

        #region Modules
        /// <summary>
        /// 백그라운드가 비활성화되는 애니메이션을 재생하는 Trigger 파라미터.
        /// </summary>
        [Header("Modules")]
        [Tooltip("백그라운드가 비활성화되는 애니메이션을 재생하는 Trigger 파라미터.")]
        public TransitionWaitModule openToLoading;

        // <summary>
        /// 백그라운드가 비활성화되는 애니메이션을 재생하는 Trigger 파라미터.
        /// </summary>
        [Tooltip("백그라운드가 비활성화되는 애니메이션을 재생하는 Trigger 파라미터.")]
        public TransitionWaitModule loadingToClose;
        #endregion

        private Coroutine loadingCoroutine;

        private void Awake()
        {
            openTrigger = !string.IsNullOrEmpty(openTriggerName) ? Animator.StringToHash(openTriggerName) : 0;
            closeTrigger = !string.IsNullOrEmpty(closeTriggerName) ? Animator.StringToHash(closeTriggerName) : 0;
        }

        private void Start()
        {
            StartCoroutine(WaitForLoadingCoroutine());
        }

        public void OpenBackground()
        {
            if (animator != null && openTrigger != 0)
            {
                animator.SetTrigger(openTrigger);
                animator.Update(0.0f);
            }
        }

        public void CloseBackground()
        {
            if (animator != null && closeTrigger != 0)
            {
                animator.SetTrigger(closeTrigger);
                animator.Update(0.0f);
            }
        }

        private IEnumerator WaitForAnimationCorotuine()
        {
            

            yield return null;
        }

        private IEnumerator WaitForLoadingCoroutine()
        {
            var nextScene = SceneManager.Instance.nextScene;
            var loadingTask = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextScene, UnityEngine.SceneManagement.LoadSceneMode.Single);

            yield return new WaitUntil(() => loadingTask.isDone);
            yield return null;
        }
    }
}
