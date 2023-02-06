using System;
using UnityEngine;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// ��� ���� Ŭ����.
    /// </summary>
    [CreateAssetMenu(fileName = "New BGM File", menuName = "Audio File/BGM", order = int.MaxValue)]
    public sealed class BGM : ScriptableObject
    {
        #region Field
        /// <summary>
        /// �ش� BGM�� ���� ���� �ҽ�.
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// �ش� BGM�� �̸�.
        /// </summary>
        public string fileName;

        /// <summary>
        /// �ش� BGM�� �ݺ� ����� ���� ����.
        /// </summary>
        public bool isLoop;

        /// <summary>
        /// �ش� BGM�� ���� ���� ����
        /// </summary>
        public float loopStart;

        /// <summary>
        /// �ش� BGM�� ���� ���� ����
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