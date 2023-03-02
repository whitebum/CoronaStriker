using UnityEngine;

namespace CoronaStriker.UI
{
    /// <summary>
    /// 액터의 체력을 표시하는 HUD 클래스들의 기본 클래스.
    /// </summary>
    public abstract class HealthViewer : MonoBehaviour
    {
        /// <summary>
        /// 체력을 표기합니다.
        /// </summary>
        /// <param name="curHealth">액터의 현재 체력.</param>
        /// <param name="maxHealth">엑터의 전체 체력.</param>
        public abstract void Display(int curHealth, int maxHealth);
    }
}