using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoronaStriker.Core.Movements
{
    /// <summary>
    /// 플레이어의 행동들을 관리하고 수행하는 매니저 클래스.
    /// </summary>
    public sealed class MoveManager : MonoBehaviour
    {
        public List<Movement> movements;

        public void Update()
        {
            if (movements != null && movements.Count > 0)
            {
                foreach (var movement in movements)
                    movement.OnUpdate();
            }
        }
    }
}
