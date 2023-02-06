using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoronaStriker.HUD;
using CoronaStriker.Core.Utils;

namespace CoronaStriker.Level
{
    /// <summary>
    /// 게임 시작 시, 짧은 인트로 애니메이션과 타이틀 스크린이 띄워지는 메인 타이틀 씬.
    /// </summary>
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] private TitleBackground background;

        [SerializeField] private 

        private enum SceneState
        {
            None,
            Introduction,
            MainTitle,
            Exit,
        }
        
        private SceneState currentState;

    }
}