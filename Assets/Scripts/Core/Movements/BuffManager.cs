using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoronaStriker.Core.Movements
{
    public sealed class BuffManager : MonoBehaviour
    {
        public List<Buff> buffs;

        private void Awake()
        {
            buffs = new List<Buff>();
            buffs.Capacity = 6;
        }

        /// <summary>
        /// 버프를 추가하고, 발동시킵니다.
        /// </summary>
        /// <param name="buff">발동시킬 버프.</param>
        public void AddBuff(Buff buff)
        {
            // 임시 코드.
            if (buffs.FirstOrDefault(x => x == buff))
                RemoveBuff(buff);

            buffs.Add(buff);
        }

        /// <summary>
        /// 발동이 끝난 버프를 제거합니다.
        /// </summary>
        /// <param name="buff"></param>
        public void RemoveBuff(Buff buff)
        {

        }
    }
}
