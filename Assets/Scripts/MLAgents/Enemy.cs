using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using MLAgents;

namespace CCG
{
    public class Enemy : MonoBehaviour
    {
        #region constants
        private const string TARGET_TAG = "Agent";
        #endregion

        #region unity callbacks
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TARGET_TAG))
            {
                // 敵と接触
                var agent = collision.GetComponent<Agent>();
                agent.AddReward(-1.0f);
                agent.AgentReset();
            }
        }
        #endregion
    }
}