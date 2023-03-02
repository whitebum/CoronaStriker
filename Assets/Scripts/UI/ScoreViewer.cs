using UnityEngine;

namespace CoronaStriker.UI.HUD
{
    /// <summary>
    /// ���ھ ǥ���ϴ� HUD Ŭ�������� �⺻ Ŭ����.
    /// </summary>
    public abstract class ScoreViewer : MonoBehaviour
    {
        /// <summary>
        /// ���ھ ǥ���մϴ�.
        /// </summary>
        /// <param name="score">ǥ���� ���ھ�.</param>
        public abstract void Display(int score);
    }
}
