using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CCG
{
    public class Stage : MonoBehaviour
    {
        #region properties
        public Vector2 StartPos { get; private set; }
        #endregion

        #region public methods
        public void LoadDebugData()
        {
            StartPos = Vector2.zero;
        }
        #endregion
    }
}