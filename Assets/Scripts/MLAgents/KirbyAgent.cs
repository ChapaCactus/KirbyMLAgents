using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using MLAgents;

namespace CCG
{
    public class KirbyAgent : Agent
    {
        #region public methods
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            base.AgentAction(vectorAction, textAction);
        }

        public override void AgentOnDone()
        {
            base.AgentOnDone();
        }

        public override void AgentReset()
        {
            base.AgentReset();
        }

        public override void CollectObservations()
        {
            base.CollectObservations();
        }

        public override void InitializeAgent()
        {
            base.InitializeAgent();
        }
        #endregion
    }
}