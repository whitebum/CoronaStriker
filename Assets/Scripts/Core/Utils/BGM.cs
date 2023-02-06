using System;
using UnityEngine;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// 배경 음악 클래스.
    /// </summary>
    [CreateAssetMenu(fileName = "New BGM File", menuName = "Audio File/BGM", order = int.MaxValue)]
    public sealed class BGM : ScriptableObject
    {
        #region Field
        /// <summary>
        /// 해당 BGM의 원본 사운드 소스.
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// 해당 BGM의 이름.
        /// </summary>
        public string fileName;

        /// <summary>
        /// 해당 BGM의 반복 재생에 대한 여부.
        /// </summary>
        public bool isLoop;

        /// <summary>
        /// 해당 BGM의 루프 시작 시점
        /// </summary>
        public float loopStart;

        /// <summary>
        /// 해당 BGM의 루프 종료 시점
        /// </summary>
        public float loopEnd;
        #endregion

        public override string ToString() 
        {
            if (isLoop)
            {
                return $"BGM Name: {fileName}\n" +
                       $"Loop : {isLoop}" +
                       $"Loop Start : {loopStart}" +
                       $"Loop End : {loopEnd}";
            }

            return $"BGM Name: {fileName}\n" +
                   $"Loop : {isLoop}";
        }
    }
}