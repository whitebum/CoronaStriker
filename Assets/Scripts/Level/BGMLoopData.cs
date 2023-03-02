using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoronaStriker.Level
{
    public sealed record LoopData
    {
        public const float defaultLoopStart = 0.0f;
        public const float defaultLoopEnd   = 1.0f;

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

        public LoopData(float _loopStart, float _loopEnd)
        {
            loopStart = _loopStart;
            loopEnd = _loopEnd;
        }
    }

    /// <summary>
    /// BGM의 반복 재생을 위한 데이터들을 담고 있는 데이터 테이블.
    /// </summary>
    [CreateAssetMenu(fileName = "New BGM Data", menuName = "Data Table", order = int.MaxValue)]
    public sealed class BGMLoopData : ScriptableObject, IEquatable<BGMLoopData>
    {
        /// <summary>
        /// 재생할 BGM의 음원.
        /// </summary>
        [Tooltip("재생할 BGM의 음원.")]
        public AudioClip clip;

        /// <summary>
        /// 재생할 BGM의 이름.
        /// </summary>
        [Tooltip("재생할 BGM의 이름.")]
        public string bgmName;

        /// <summary>
        /// 재생할 BGM의 재생 시작 구간.
        /// </summary>
        [Tooltip("재생할 BGM의 재생 시작 구간.")]
        public float startFrom;

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
            bgmName = "Unknown BGM";
            startFrom = 0.0f;
            loopStart = 0.0f;
            loopEnd = 999.9f;
        }

        public bool Equals(BGMLoopData other)
        {
            return Equals(clip, other.clip) && string.Equals(bgmName, other.bgmName) &&
                  startFrom.Equals(other.startFrom) && loopStart.Equals(loopStart) && loopEnd.Equals(loopEnd);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            return other is BGMLoopData && Equals(other as BGMLoopData);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = clip != null ? clip.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(bgmName) ? bgmName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (!float.IsNaN(startFrom) ? startFrom.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (!float.IsNaN(loopStart) ? loopStart.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (!float.IsNaN(loopEnd) ? loopEnd.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"현재 \"{bgmName}\" 재생중... ({startFrom}에서 시작)\n" +
                   $"루프 시작 지점: {loopStart} ,, 종료 지점: {loopEnd}";
        }

        public static bool operator ==(BGMLoopData left, BGMLoopData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BGMLoopData left, BGMLoopData right)
        {
            return !left.Equals(right);
        }
    }
}
