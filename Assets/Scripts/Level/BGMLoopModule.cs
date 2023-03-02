using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.Level
{
    /// <summary>
    /// BGM을 반복 재생하는 모듈 클래스.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class BGMLoopModule : MonoBehaviour
    {
        /// <summary>
        /// BGM을 재생할 Audio Source.
        /// </summary>
        [SerializeField]
        public AudioSource source;

        /// <summary>
        /// 재생할 BGM의 반복 재생 시작 구간.
        /// </summary>
        [Tooltip("재생할 BGM의 반복 재생 시작 구간.")]
        public float loopStart;

        /// <summary>
        /// 재생할 BGM의 반복 재생 종료 구간.
        /// </summary>
        [Tooltip("재생할 BGM의 반복 재생 종료 구간.")]
        public float loopEnd;

        private void Reset()
        {
            source = GetComponent<AudioSource>();
        }

        private void Awake()
        {
            source = source ?? GetComponent<AudioSource>();

            loopStart = 0.000f;
            loopEnd = 999.9f;
        }

        public void Update()
        {
            if (source == null) return;
            if (source.isPlaying == true && source.time >= loopEnd)
            {
                source.time = loopStart;
            }
        }

        /// <summary>
        /// BGM을 반복 재생할 수 있도록 환경을 구축합니다.
        /// </summary>
        /// <param name="data">반복 재생할 BGM의 데이터 테이블.</param>
        public void SetFrom(BGMLoopData data)
        {
            source.clip = data.clip;
            source.time = !float.IsNaN(data.startFrom) ? data.startFrom : 0.0f;

            loopStart = !float.IsNaN(data.loopStart) ? data.loopStart : 0.0f;
            loopEnd = !float.IsNaN(data.loopEnd) ? data.loopEnd : 999.9f;
        }
    }
}
