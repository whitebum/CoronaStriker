using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// 효과음 클래스.
    /// </summary>
    [CreateAssetMenu(fileName = "New SFX File", menuName = "Audio File/SFX", order = int.MaxValue)]
    public sealed class SFX : ScriptableObject
    {
        #region Field
        /// <summary>
        /// 해당 SFX의 원본 사운드 소스.
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// 해당 SFX의 이름.
        /// </summary>
        public string fileName;
        #endregion

        public override string ToString()
        {
            return $"SFX Name: {fileName}\n";
        }
    }
}