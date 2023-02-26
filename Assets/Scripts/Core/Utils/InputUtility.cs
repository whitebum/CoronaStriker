using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// ���� ������ ���Ǵ� ��� Ű�� �����ϴ� ���� Ŭ����.
    /// </summary>
    public static class InputUtility
    {
        /// <summary>
        /// Ű�� �������� ��� ������ ���̺�.
        /// </summary>
        public sealed record InputData
        {
            /// <summary>
            /// ����� KeyCode�� �����͸� �ҷ��� �� ���Ǵ� ���ڿ� Ű.
            /// </summary>
            public string key;

            /// <summary>
            /// ����� KeyCode.
            /// </summary>
            public KeyCode keyCode
            {
                get => PlayerPrefs.HasKey(key) ? (KeyCode)PlayerPrefs.GetInt(key) : KeyCode.None;
                set => PlayerPrefs.SetInt(key, (int)value);
            }

            public InputData(string key, KeyCode keyCode)
            {
                this.key = key;
                this.keyCode = keyCode;
            }

            public override string ToString()
            {
                return $"Key: {key} Value: {keyCode}";
            }

            public static implicit operator KeyCode(InputData data) => data.keyCode;

            public static implicit operator int(InputData data) => (int)data.keyCode;
        }

        #region Movement Key
        /// <summary>
        /// �� �������� �̵��ϴ� Ű. ���� ���� '���� ȭ��ǥ'�Դϴ�.
        /// </summary>
        public static InputData upKey = new InputData("Up", KeyCode.UpArrow);

        /// <summary>
        /// �Ʒ� �������� �̵��ϴ� Ű. ���� ���� '�Ʒ��� ȭ��ǥ'�Դϴ�.
        /// </summary>
        public static InputData downKey = new InputData("Down", KeyCode.DownArrow);

        /// <summary>
        /// ���� �������� �̵��ϴ� Ű. ���� ���� '���� ȭ��ǥ'�Դϴ�.
        /// </summary>
        public static InputData leftKey = new InputData("Left", KeyCode.LeftArrow);

        /// <summary>
        /// ������ �������� �̵��ϴ� Ű. ���� ���� '������ ȭ��ǥ'�Դϴ�.
        /// </summary>
        public static InputData rightKey = new InputData("Right", KeyCode.RightArrow);

        /// <summary>
        /// ���� Ű, ���� ���� '���콺 ���� ��ư'�Դϴ�.
        /// </summary>
        public static InputData attackKey = new InputData("Attack", KeyCode.Mouse0);
        #endregion

        #region Menu Key
        /// <summary>
        /// �޴��� �����ϴ� Ű. ���� ���� '���� Ű'�Դϴ�.
        /// </summary>
        public static InputData selectKey = new InputData("Select", KeyCode.Return);

        /// <summary>
        /// �ڷ� ���ų� �Ͻ����� �ϴ� Ű, ���� ���� 'Esc Ű'�Դϴ�.
        /// </summary>
        public static InputData escapeKey = new InputData("Escape", KeyCode.Escape);
        #endregion

        #region Other Key
        /// <summary>
        /// �޴��� �����ϴ� Ű. ���� ���� '���� Ű'�Դϴ�.
        /// </summary>
        public static InputData cheatKey = new InputData("Cheat", KeyCode.LeftControl);
        #endregion
    }
}