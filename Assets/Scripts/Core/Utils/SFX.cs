using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// ȿ���� Ŭ����.
    /// </summary>
    [CreateAssetMenu(fileName = "New SFX File", menuName = "Audio File/SFX", order = int.MaxValue)]
    public sealed class SFX : ScriptableObject
    {
        #region Field
        /// <summary>
        /// �ش� SFX�� ���� ���� �ҽ�.
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// �ش� SFX�� �̸�.
        /// </summary>
        public string fileName;
        #endregion

        public override string ToString()
        {
            return $"SFX Name: {fileName}\n";
        }
    }
}