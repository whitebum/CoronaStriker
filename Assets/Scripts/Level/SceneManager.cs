using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CoronaStriker.Core.Utils;
using System.Linq;

namespace CoronaStriker.Level
{
    /// <summary>
    /// Scene간의 이동을 진행하는 매니저 클래스.
    /// </summary>
    public sealed class SceneManager : Singleton<SceneManager>
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public List<string> scenes;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [HideInInspector] public string nextScene;

        public void LoadScene(string scene)
        {
            nextScene = scenes.FirstOrDefault((_scene) => _scene.Equals(scene));
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        }
    }
}