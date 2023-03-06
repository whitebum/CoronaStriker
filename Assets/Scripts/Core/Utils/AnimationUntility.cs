using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoronaStriker.Core.Utils
{
    public sealed class AnimationUntility : MonoBehaviour
    {
        public void Distroy()
        {
            Destroy(gameObject);
        }

        public void Distory(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        public void SetEnable()
        {
            gameObject.SetActive(true);
        }

        public void SetEnable(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        public void SetDisable()
        {
            gameObject.SetActive(false);
        }

        public void SetDisable(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
