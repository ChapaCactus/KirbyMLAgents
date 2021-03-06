﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using MLAgents;

namespace CCG
{
    public class Goal : MonoBehaviour
    {
        #region constants
        private const string TARGET_TAG = "Agent";
        #endregion

        #region unity callbacks
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(TARGET_TAG))
            {
                // GOAL!
                var agent = collision.GetComponent<Agent>();
                agent.AddReward(1000.0f);
                agent.Done();
            }
        }
        #endregion
    }
}