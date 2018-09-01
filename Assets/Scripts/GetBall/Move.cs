using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CCG
{
    public class Move
    {
        #region properties
        public int Value { get; private set; }
        #endregion

        #region public methods
        public static Move CreateRightMove()
        {
            return new Move(1);
        }

        public static Move CreateLeftMove()
        {
            return new Move(-1);
        }

        public Move(int value)
        {
            Value = value;
        }
        #endregion
    }
}