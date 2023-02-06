using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoronaStriker.HUD;
using CoronaStriker.Core.Utils;

namespace CoronaStriker.Level
{
    /// <summary>
    /// ���� ���� ��, ª�� ��Ʈ�� �ִϸ��̼ǰ� Ÿ��Ʋ ��ũ���� ������� ���� Ÿ��Ʋ ��.
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