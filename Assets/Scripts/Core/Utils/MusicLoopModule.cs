using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// BGM을 반복 재생하는 클래스.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class MusicLoopModule : MonoBehaviour
    {
        /// <summary>
        /// BGM을 재생할 Audio Source.
        /// </summary>
        [Tooltip("BGM을 재생할 Audio Source.")]
        [SerializeField] private AudioSource audioSource;

        /// <summary>
        /// 재생되는 곡의 반복 시점.
        /// </summary>
        [Tooltip("재생되는 곡의 반복 시점.")]
        public float loopStart;

        /// <summary>
        /// 재생되는 곡의 끝.
        /// </summary>
        [Tooltip("재생되는 곡의 끝.")]
        public float loopEnd;

        private void Reset()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        private void Awake()
        {
            if (audioSource == null)
                audioSource= GetComponentInChildren<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (loopStart <= 0.0f && loopEnd <= 0.0f) return;
            
            // BGM을 반복 재생합니다.
            if (audioSource != null && audioSource.time >= loopEnd)
            {
                audioSource.time = loopStart;
            }
        }

        /// <summary>
        /// 반복 재생할 BGM의 데이터를 읽어옵니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void SetFrom(BGM bgm)
        {
            loopStart = bgm.loopStart;
            loopEnd = bgm.loopEnd;
        }
    }
}
