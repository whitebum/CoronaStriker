using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoronaStriker.Level
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "New Scene", menuName = "Data Table/Scene Data", order = int.MaxValue)]
    public class SceneData : ScriptableObject
    {
        public string sceneName;
    }
}
