using CoronaStriker.Core.Utils;
using CoronaStriker.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.UI
{
    /// <summary>
    /// 타이틀 스크린 클래스.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TitleScreen : MonoBehaviour
    {
        /// <summary>
        /// 오프닝이 끝났는 지에 대한 여부.
        /// </summary>
        private bool isEndedOpening;

        #region Animation
        /// <summary>
        /// 애니메이션을 재생하는 Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("애니메이션을 재생하는 Animator.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// 오프닝 애니메이션.
        /// </summary>
        [Space]
        [Tooltip("오프닝 애니메이션.")]
        [SerializeField] private string openingTrigger;

        /// <summary>
        /// 오프닝 애니메이션.
        /// </summary>
        private int openingTriggerHash;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("오프닝 애니메이션.")]
        [SerializeField] private string openingExitTrigger;

        /// <summary>
        /// 
        /// </summary>
        private int openingExitTriggerHash;

        /// <summary>
        /// 메인 타이틀 애니메이션.
        /// </summary>
        [Tooltip("메인 타이틀 애니메이션.")]
        [SerializeField] private string titleTrigger;

        /// <summary>
        /// 메인 타이틀 애니메이션.
        /// </summary>
        private int titleTriggerHash;

        /// <summary>
        /// 메인 타이틀 애니메이션.
        /// </summary>
        [Tooltip("메인 타이틀 애니메이션.")]
        [SerializeField] private string titleExitTrigger;

        /// <summary>
        /// 메인 타이틀 애니메이션.
        /// </summary>
        private int titleExitTriggerHash;

        /// <summary>
        /// 
        /// </summary>
        private Coroutine animationCoroutine;
        #endregion

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            isEndedOpening = false;

            animator = animator ?? GetComponent<Animator>();

            openingTriggerHash = !string.IsNullOrEmpty(openingTrigger) ? Animator.StringToHash(openingTrigger) : 0;
            openingExitTriggerHash = !string.IsNullOrEmpty(openingExitTrigger) ? Animator.StringToHash(openingExitTrigger) : 0;
            titleTriggerHash = !string.IsNullOrEmpty(titleTrigger) ? Animator.StringToHash(titleTrigger) : 0;
            titleExitTriggerHash = !string.IsNullOrEmpty(titleExitTrigger) ? Animator.StringToHash(titleExitTrigger) : 0;
        }

        private void Start()
        {
            //Opening();
            isEndedOpening = true;

            OpeningExit();
            Title();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isEndedOpening == false)
                {
                    isEndedOpening = true;

                    if (animationCoroutine != null)
                    {
                        StopCoroutine(animationCoroutine);
                        animationCoroutine = null;
                    }
                    OpeningExit();
                }
                else
                {
                    TitleExit();
                    SceneManager.Instance.LoadScene("Test");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Opening()
        {
            if (animator != null && openingTriggerHash != 0)
            {
                animator.SetTrigger(openingTriggerHash);
                animator.Update(0.0f);
            }

            animationCoroutine = StartCoroutine(WaitForOpeningCoroutine(OpeningExit));
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpeningExit()
        {
            if (animator != null)
            {
                if (openingExitTriggerHash != 0)
                {
                    animator.SetTrigger(openingExitTriggerHash);
                    animator.Update(0.0f);
                }
            }

            Title();
        }

        private void Title()
        {
            if (animator != null)
            {
                if (titleTriggerHash != 0)
                {
                    animator.SetTrigger(titleTriggerHash);
                    animator.Update(0.0f);
                }
            }
        }

        private void TitleExit()
        {
            if (animator != null && titleExitTriggerHash != 0)
            {
                animator.SetTrigger(titleExitTriggerHash);
                animator.Update(0.0f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator WaitForOpeningCoroutine(UnityAction callback)
        {
            if (animator != null && !string.IsNullOrEmpty(openingTrigger))
            {
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f &&
                                                 animator.GetCurrentAnimatorStateInfo(0).IsName(openingTrigger));
            }

            callback.Invoke();

            yield return null;
        }
    }
}