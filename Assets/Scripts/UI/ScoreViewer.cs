using UnityEngine;

namespace CoronaStriker.UI.HUD
{
    /// <summary>
    /// 스코어를 표기하는 HUD 클래스들의 기본 클래스.
    /// </summary>
    public abstract class ScoreViewer : MonoBehaviour
    {
        /// <summary>
        /// 스코어를 표기합니다.
        /// </summary>
        /// <param name="score">표기할 스코어.</param>
        public abstract void Display(int score);
    }
}
