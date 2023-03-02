using UnityEngine;

namespace CoronaStriker.UI
{
    /// <summary>
    /// ������ ü���� ǥ���ϴ� HUD Ŭ�������� �⺻ Ŭ����.
    /// </summary>
    public abstract class HealthViewer : MonoBehaviour
    {
        /// <summary>
        /// ü���� ǥ���մϴ�.
        /// </summary>
        /// <param name="curHealth">������ ���� ü��.</param>
        /// <param name="maxHealth">������ ��ü ü��.</param>
        public abstract void Display(int curHealth, int maxHealth);
    }
}