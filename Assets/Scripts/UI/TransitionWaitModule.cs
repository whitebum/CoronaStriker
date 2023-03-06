using CoronaStriker.Level;
using System;
using UnityEditor.EditorTools;
using UnityEngine;

namespace CoronaStriker.UI
{
    /// <summary>
    /// 작업(애니메이션 재생, 일정 시간 대기, 오디오 재생)을 대기할 때 사용되는 모듈 구조체.
    /// </summary>
    [Serializable]
    public struct TransitionWaitModule : IEquatable<TransitionWaitModule>
    {
        /// <summary>
        /// 종료될 때까지 대기할 애니메이션의 파라미터 이름.
        /// </summary>
        [Tooltip("대기할 애니메이션을 재생하기 위한 파라미터 이름.")]
        public string animation;

        /// <summary>
        /// 대기할 일정 시간.
        /// </summary>
        [Tooltip("대기할 일정 시간.")]
        public float time;

        /// <summary>
        /// 종료될 때까지 대기할 오디오.
        /// </summary>
        [Tooltip("종료될 때까지 대기할 오디오.")]
        public AudioClip audio;

        /// <summary>
        /// 종료될 때까지 대기할 Transition.
        /// </summary>
        [Tooltip("종료될 때까지 대기할 Transition.")]
        public Transition transition;

        public bool Equals(TransitionWaitModule other)
        {
            return animation.Equals(other.animation) && time.Equals(other.time) && Equals(audio, other.audio);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TransitionWaitModule && Equals((TransitionWaitModule)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = !string.IsNullOrEmpty(animation) ? animation.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (!float.IsNaN(time) ? time.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (audio != null ? audio.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (transition != null ? transition.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(TransitionWaitModule left, TransitionWaitModule right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TransitionWaitModule left, TransitionWaitModule right)
        {
            return !left.Equals(right);
        }
    }
}
