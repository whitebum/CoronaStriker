using CoronaStriker.Core.Utils;
using CoronaStriker.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoronaStriker.UI
{
    /// <summary>
    /// Ÿ��Ʋ ��ũ�� Ŭ����.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TitleScreen : MonoBehaviour
    {
        /// <summary>
        /// �������� ������ ���� ���� ����.
        /// </summary>
        private bool isEndedOpening;

        #region Animation
        /// <summary>
        /// �ִϸ��̼��� ����ϴ� Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("�ִϸ��̼��� ����ϴ� Animator.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// ������ �ִϸ��̼�.
        /// </summary>
        [Space]
        [Tooltip("������ �ִϸ��̼�.")]
        [SerializeField] private string openingTrigger;

        /// <summary>
        /// ������ �ִϸ��̼�.
        /// </summary>
        private int openingTriggerHash;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("������ �ִϸ��̼�.")]
        [SerializeField] private string openingExitTrigger;

        /// <summary>
        /// 
        /// </summary>
        private int openingExitTriggerHash;

        /// <summary>
        /// ���� Ÿ��Ʋ �ִϸ��̼�.
        /// </summary>
        [Tooltip("���� Ÿ��Ʋ �ִϸ��̼�.")]
        [SerializeField] private string titleTrigger;

        /// <summary>
        /// ���� Ÿ��Ʋ �ִϸ��̼�.
        /// </summary>
        private int titleTriggerHash;

        /// <summary>
        /// ���� Ÿ��Ʋ �ִϸ��̼�.
        /// </summary>
        [Tooltip("���� Ÿ��Ʋ �ִϸ��̼�.")]
        [SerializeField] private string titleExitTrigger;

        /// <summary>
        /// ���� Ÿ��Ʋ �ִϸ��̼�.
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