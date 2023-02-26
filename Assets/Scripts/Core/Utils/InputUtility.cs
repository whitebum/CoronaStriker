using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.Core.Utils
{
    /// <summary>
    /// 게임 내에서 사용되는 모든 키를 관리하는 정적 클래스.
    /// </summary>
    public static class InputUtility
    {
        /// <summary>
        /// 키의 설정값을 담는 데이터 테이블.
        /// </summary>
        public sealed record InputData
        {
            /// <summary>
            /// 저장된 KeyCode의 데이터를 불러올 때 사용되는 문자열 키.
            /// </summary>
            public string key;

            /// <summary>
            /// 저장된 KeyCode.
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
        /// 위 방향으로 이동하는 키. 고정 값은 '위쪽 화살표'입니다.
        /// </summary>
        public static InputData upKey = new InputData("Up", KeyCode.UpArrow);

        /// <summary>
        /// 아래 방향으로 이동하는 키. 고정 값은 '아래쪽 화살표'입니다.
        /// </summary>
        public static InputData downKey = new InputData("Down", KeyCode.DownArrow);

        /// <summary>
        /// 왼쪽 방향으로 이동하는 키. 고정 값은 '왼쪽 화살표'입니다.
        /// </summary>
        public static InputData leftKey = new InputData("Left", KeyCode.LeftArrow);

        /// <summary>
        /// 오른쪽 방향으로 이동하는 키. 고정 값은 '오른쪽 화살표'입니다.
        /// </summary>
        public static InputData rightKey = new InputData("Right", KeyCode.RightArrow);

        /// <summary>
        /// 공격 키, 고정 값은 '마우스 왼쪽 버튼'입니다.
        /// </summary>
        public static InputData attackKey = new InputData("Attack", KeyCode.Mouse0);
        #endregion

        #region Menu Key
        /// <summary>
        /// 메뉴를 선택하는 키. 고정 값은 '엔터 키'입니다.
        /// </summary>
        public static InputData selectKey = new InputData("Select", KeyCode.Return);

        /// <summary>
        /// 뒤로 가거나 일시정지 하는 키, 고정 값은 'Esc 키'입니다.
        /// </summary>
        public static InputData escapeKey = new InputData("Escape", KeyCode.Escape);
        #endregion

        #region Other Key
        /// <summary>
        /// 메뉴를 선택하는 키. 고정 값은 '엔터 키'입니다.
        /// </summary>
        public static InputData cheatKey = new InputData("Cheat", KeyCode.LeftControl);
        #endregion
    }
}