using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoronaStriker.Core.Movements
{
    public class TestMovement : Movement
    {
        private const float litaralMoveSpeed = 10.0f;

        public override void OnUpdate()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            transform.position += new Vector3(horizontal, vertical, 0.0f) * (litaralMoveSpeed * Time.deltaTime);
        }
    }
}
