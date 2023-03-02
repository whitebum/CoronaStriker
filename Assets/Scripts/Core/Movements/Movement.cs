using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoronaStriker.Core.Movements
{
    /// <summary>
    /// 액터가 취하는 모든 행동들을 구현하는 클래스들의 기본 클래스.
    /// </summary>
    public abstract class Movement : MonoBehaviour
    {
        public abstract void OnUpdate();
    }
}
