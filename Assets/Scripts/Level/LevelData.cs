using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.Level
{
    /// <summary>
    /// 각 레벨에 맞는 데이터들을 담는 데이터 테이블.
    /// </summary>
    public sealed class LevelData : ScriptableObject
    {
        public string levelName;

        public string sceneName;
    }
}
