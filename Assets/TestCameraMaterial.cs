using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public class TestCameraMaterial : MonoBehaviour 
{
        public Material material;

        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, material);
        }
    }
