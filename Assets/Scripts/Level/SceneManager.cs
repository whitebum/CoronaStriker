using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CoronaStriker.Core.Utils;

namespace CoronaStriker.Level
{
    /// <summary>
    /// Scene간의 이동을 진행하는 매니저 클래스.
    /// </summary>
    public sealed class SceneManager : Singleton<SceneManager>
    {
        [SerializeField] private Animator animator;

        [SerializeField] private string openTrigger;
        private int openTriggerHash;

        [SerializeField] private string closeTrigger;
        private int closeTriggerHash;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        protected override void Awake()
        {
            base.Awake();

            animator = animator ?? GetComponent<Animator>();

            openTriggerHash = !string.IsNullOrEmpty(openTrigger) ? Animator.StringToHash(openTrigger) : 0;
            closeTriggerHash = !string.IsNullOrEmpty(closeTrigger) ? Animator.StringToHash(closeTrigger) : 0;
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName) 
        {
            if (animator != null && openTriggerHash != 0)
            {
                animator.SetTrigger(openTriggerHash);
                animator.Update(0.0f);

                if (!string.IsNullOrEmpty(openTrigger))
                {
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f && 
                                                     animator.GetCurrentAnimatorStateInfo(0).IsName(openTrigger));
                }
            }

            var task = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

            while (true)
            {
                if (task.isDone)
                {
                    if (animator != null && closeTriggerHash != 0)
                    {
                        animator.SetTrigger(closeTriggerHash);
                        animator.Update(0.0f);

                        if (!string.IsNullOrEmpty(closeTrigger))
                        {
                            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f &&
                                                             animator.GetCurrentAnimatorStateInfo(0).IsName(closeTrigger));
                        }
                    }

                    yield break;
                }

                yield return null;
            }
        }
    }
}