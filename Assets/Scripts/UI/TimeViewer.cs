using System;using UnityEngine;

namespace CoronaStriker.UI.HUD
{
    /// <summary>
    /// 집계된 시간을 표기하는 HUD 클래스들의 기본 클래스.
    /// </summary>
    public abstract class TimeViewer : MonoBehaviour
    {
        /// <summary>
        /// 집계된 시간을 표기합니다.
        /// </summary>
        /// <param name="time">표기할 시간.(실수형)</param>
        public abstract void Display(float time);
    }
}
