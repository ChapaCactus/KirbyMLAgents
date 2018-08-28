using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using MLAgents;

namespace CCG
{
    public class KirbyDecision : MonoBehaviour, Decision
    {
        #region public methods
        public float[] Decide(
            List<float> vectorObs,
            List<Texture2D> visualObs,
            float reward,
            bool done,
            List<float> memory)
        {
            return new float[1] { UnityEngine.Random.Range(0, 4) };
        }

        public List<float> MakeMemory(
            List<float> vectorObs,
            List<Texture2D> visualObs,
            float reward,
            bool done,
            List<float> memory)
        {
            return new List<float>();
        }
        #endregion
    }
}