using Assets.Scripts.UI;
using CoronaStriker.Level;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.UI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TitleScreen : Transition
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                SceneManager.Instance.LoadScene("Test");
        }
    }
}
